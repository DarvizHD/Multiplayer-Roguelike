using ENet;
using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.ECS.Systems.Battle;
using Runtime.ECS.Systems.Movement;
using Runtime.ECS.Systems.Rotation;
using Runtime.ECS.Systems.Rotation.Runtime.ECS.Systems;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Shared.Commands;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECSWorld EcsWorld { get; } = new ();
        public MonoBehaviorProvider PlayerPrefab;
        public MonoBehaviorProvider EnemyPrefab;
        private PlayerControls _playerControls;

        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private async void Start()
        {
            _playerControls = new PlayerControls();

            _playerControls.Enable();

            EcsWorld.RegisterComponent<PositionComponent>();
            EcsWorld.RegisterComponent<RotationComponent>();
            EcsWorld.RegisterComponent<VelocityComponent>();
            EcsWorld.RegisterComponent<SpeedComponent>();
            EcsWorld.RegisterComponent<DirectionComponent>();

            EcsWorld.RegisterComponent<AttackCooldownComponent>();
            EcsWorld.RegisterComponent<MeleeAttackComponent>();

            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<EnemyTagComponent>();

            EcsWorld.RegisterComponent<PendingDamageEventComponent>();
            EcsWorld.RegisterComponent<AttackEventComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();

            EcsWorld.RegisterComponent<SeparationComponent>();
            EcsWorld.RegisterComponent<PlayerInputComponent>();

            EcsWorld.RegisterComponent<DirectionRotationComponent>();
            EcsWorld.RegisterComponent<PlayerLookRotationComponent>();

            EcsWorld.RegisterComponent<AnimatorComponent>();
            EcsWorld.RegisterComponent<HealthComponent>();
            EcsWorld.RegisterComponent<RegenerationComponent>();
            EcsWorld.RegisterComponent<DeathComponent>();
            EcsWorld.RegisterComponent<InvulnerabilityComponent>();

            var playerEntityId = 0;

            var playerProvider = Instantiate(PlayerPrefab);

            EcsWorld.AddEntityComponent(playerEntityId, new PositionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new RotationComponent());
            EcsWorld.AddEntityComponent(playerEntityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new TransformComponent((playerProvider.Transform)));
            EcsWorld.AddEntityComponent(playerEntityId, new SpeedComponent(8f));
            EcsWorld.AddEntityComponent(playerEntityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(playerEntityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerLookRotationComponent(10f));
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerInputComponent(_playerControls));
            EcsWorld.AddEntityComponent(playerEntityId, new AnimatorComponent(playerProvider.Animator));
            EcsWorld.AddEntityComponent(playerEntityId, new HealthComponent(100f));
            EcsWorld.AddEntityComponent(playerEntityId, new RegenerationComponent(5f, 3f));

            for (var i = 1; i < 2; i++)
            {
              var enemyId = i;

              var enemyProvider = Instantiate(EnemyPrefab);

              EcsWorld.AddEntityComponent(enemyId, new PositionComponent(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f))));
              EcsWorld.AddEntityComponent(enemyId, new RotationComponent());
              EcsWorld.AddEntityComponent(enemyId, new DirectionComponent(Vector3.forward));
              EcsWorld.AddEntityComponent(enemyId, new SpeedComponent(1f));
              EcsWorld.AddEntityComponent(enemyId, new TransformComponent(enemyProvider.Transform));
              EcsWorld.AddEntityComponent(enemyId, new EnemyTagComponent());
              EcsWorld.AddEntityComponent(enemyId, new DirectionRotationComponent(10f));
              EcsWorld.AddEntityComponent(enemyId, new FollowComponent(playerProvider.Transform));
              EcsWorld.AddEntityComponent(enemyId, new SeparationComponent());
              EcsWorld.AddEntityComponent(enemyId, new AnimatorComponent(enemyProvider.Animator));
              EcsWorld.AddEntityComponent(enemyId, new HealthComponent(50f));
              EcsWorld.AddEntityComponent(enemyId, new RegenerationComponent(2f, 5f));
            }

            EcsWorld.AddSystem<PlayerInputMovementSystem>();
            EcsWorld.AddSystem<FollowSystem>();
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<SeparationSystem>();
            EcsWorld.AddSystem<DirectionRotationSystem>();
            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<DeathSystem>();
            EcsWorld.AddSystem<RegenerationSystem>();
            EcsWorld.AddSystem<InvulnerabilitySystem>();
            EcsWorld.AddSystem<AttackCooldownSystem>();
            EcsWorld.AddSystem<MeleeAttackAnimationSystem>();
            EcsWorld.AddSystem<AttackSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();
            EcsWorld.AddSystem<PlayerLookRotationSystem>();

            Library.Initialize();

            var serverConnectionModel = new ServerConnectionModel();
            var serverConnectionPresenter = new ServerConnectionPresenter(serverConnectionModel, _gameFixedSystemCollection);
            serverConnectionPresenter.Enable();

            serverConnectionModel.ConnectPlayer();
            await serverConnectionModel.CompletePlayerConnectAwaiter;

            var loginCommand = new LoginCommand("Varfolomey");
            loginCommand.Write(serverConnectionModel.PlayerPeer);

            var createLobbyCommand = new CreateLobbyCommand("Varfolomey");
            createLobbyCommand.Write(serverConnectionModel.PlayerPeer);

            OnDrawGizmos();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            var results = EcsWorld.ComponentManager.Query(
                typeof(PositionComponent),
                typeof(RotationComponent),
                typeof(MeleeAttackComponent)
            );

            foreach (var (id, components) in results)
            {
                var position = (PositionComponent)components[0];
                var rotation = (RotationComponent)components[1];
                var melee = (MeleeAttackComponent)components[2];

                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawWireSphere(position.Position, melee.Range);

                var attackDir = Quaternion.Euler(0, rotation.Angle, 0) * Vector3.forward;
                attackDir.y = 0;
                attackDir.Normalize();

                var halfAngle = melee.Angle * 0.5f;
                var leftDir = Quaternion.Euler(0, -halfAngle, 0) * attackDir;
                var rightDir = Quaternion.Euler(0, halfAngle, 0) * attackDir;

                Gizmos.color = Color.red;
                Gizmos.DrawRay(position.Position, leftDir * melee.Range);
                Gizmos.DrawRay(position.Position, rightDir * melee.Range);

                var steps = 20;
                var prev = position.Position + leftDir * melee.Range;
                for (var i = 1; i <= steps; i++)
                {
                    var t = (float)i / steps;
                    var angle = Mathf.Lerp(-halfAngle, halfAngle, t);
                    var dir = Quaternion.Euler(0, angle, 0) * attackDir;
                    var next = position.Position + dir * melee.Range;
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
            }
        }

        private void Update()
        {
            EcsWorld.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }
    }
}
