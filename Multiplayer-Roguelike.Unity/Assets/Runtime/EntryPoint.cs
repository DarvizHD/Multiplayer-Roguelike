using ENet;
using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Camera;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Components.Spawn;
using Runtime.ECS.Components.Tags;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.ECS.Systems.Battle;
using Runtime.ECS.Systems.Battle.MeleeAttack;
using Runtime.ECS.Systems.CameraFocus;
using Runtime.ECS.Systems.Follow;
using Runtime.ECS.Systems.Movement;
using Runtime.ECS.Systems.Rotation;
using Runtime.ECS.Systems.Rotation.Runtime.ECS.Systems;
using Runtime.ECS.Systems.Spawn;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Shared.Commands;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECSWorld EcsWorld { get; } = new();
        public MonoBehaviorProvider PlayerPrefab;
        public MonoBehaviorProvider EnemyPrefab;
        public MonoBehaviorProvider CameraTarget;
        public GizmosHelper GizmosHelper;
        private PlayerControls _playerControls;

        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private async void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();

            GizmosHelper.Initialize(EcsWorld);

            RegisterComponents();

            var playerEntityId = 0;
            var playerProvider = Instantiate(PlayerPrefab);
            CreatePlayer(playerEntityId, playerProvider, Vector3.zero);
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerInputComponent(_playerControls));

            //TODO: Test second player
            var testPlayerEntityId = 1;
            var testplayerProvider = Instantiate(PlayerPrefab);
            CreatePlayer(testPlayerEntityId, testplayerProvider, new Vector3(15f, 0f, 5f));

            var cameraTargetEntityId = 2;
            EcsWorld.AddEntityComponent(cameraTargetEntityId, new CameraTargetComponent());
            EcsWorld.AddEntityComponent(cameraTargetEntityId, new TransformComponent(CameraTarget.transform));

            for (var i = 3; i < 100; i++)
            {
                CreateEnemy(i, playerProvider);
            }

            var spawnerEntityId = 100;
            EcsWorld.AddEntityComponent(spawnerEntityId, new SpawnerComponent(
                targetCount: 2,
                prefab: EnemyPrefab.gameObject,
                centerPosition: new Vector3(20f, 0f, 20f),
                spawnRadius: 8f
            ));

            AddSystems();

            Library.Initialize();

            var serverConnectionModel = new ServerConnectionModel();
            var serverConnectionPresenter =
                new ServerConnectionPresenter(serverConnectionModel, _gameFixedSystemCollection);
            serverConnectionPresenter.Enable();

            serverConnectionModel.ConnectPlayer();
            await serverConnectionModel.CompletePlayerConnectAwaiter;

            var loginCommand = new LoginCommand("Varfolomey");
            loginCommand.Write(serverConnectionModel.PlayerPeer);

            var createLobbyCommand = new CreateLobbyCommand("Varfolomey");
            createLobbyCommand.Write(serverConnectionModel.PlayerPeer);
        }

        private void RegisterComponents()
        {
            EcsWorld.RegisterComponent<PositionComponent>();
            EcsWorld.RegisterComponent<RotationComponent>();
            EcsWorld.RegisterComponent<VelocityComponent>();
            EcsWorld.RegisterComponent<DirectionComponent>();

            EcsWorld.RegisterComponent<MoveSpeedComponent>();
            EcsWorld.RegisterComponent<RotationSpeedComponent>();

            EcsWorld.RegisterComponent<AttackCooldownComponent>();
            EcsWorld.RegisterComponent<MeleeAttackComponent>();

            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<EnemyTagComponent>();

            EcsWorld.RegisterComponent<PendingDamageEventComponent>();
            EcsWorld.RegisterComponent<AttackEventComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();

            EcsWorld.RegisterComponent<SeparationComponent>();
            EcsWorld.RegisterComponent<PlayerInputComponent>();
            EcsWorld.RegisterComponent<PlayerTagComponent>();

            EcsWorld.RegisterComponent<DirectionRotationTagComponent>();
            EcsWorld.RegisterComponent<PlayerLookRotationTagComponent>();

            EcsWorld.RegisterComponent<AnimatorComponent>();
            EcsWorld.RegisterComponent<HealthComponent>();
            EcsWorld.RegisterComponent<RegenerationComponent>();
            EcsWorld.RegisterComponent<DeathTagComponent>();
            EcsWorld.RegisterComponent<InvulnerabilityComponent>();
            EcsWorld.RegisterComponent<SpawnerComponent>();
            EcsWorld.RegisterComponent<SpawnedUnitTagComponent>();
            EcsWorld.RegisterComponent<DeathAnimationComponent>();
            EcsWorld.RegisterComponent<GameObjectComponent>();

            EcsWorld.RegisterComponent<CameraTargetComponent>();
            EcsWorld.RegisterComponent<FreezeMovementComponent>();
            EcsWorld.RegisterComponent<FreezeMovementByDamageComponent>();
        }

        private void AddSystems()
        {
            EcsWorld.AddSystem<PlayerInputMovementSystem>();
            EcsWorld.AddSystem<FollowSystem>();
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<FollowSeparationSystem>();
            EcsWorld.AddSystem<DirectionRotationSystem>();
            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();

            EcsWorld.AddSystem<RegenerationSystem>();
            EcsWorld.AddSystem<InvulnerabilitySystem>();

            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();
            EcsWorld.AddSystem<MeleeAttackAnimationSystem>();

            EcsWorld.AddSystem<AttackCooldownSystem>();
            EcsWorld.AddSystem<AttackSystem>();

            EcsWorld.AddSystem<DamageAnimationSystem>();

            EcsWorld.AddSystem<FreezeMovementByDamageSystem>();

            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<DeathSystem>();

            EcsWorld.AddSystem<PlayerLookRotationSystem>();
            EcsWorld.AddSystem<CameraFocusSystem>();
            EcsWorld.AddSystem<DrawCameraTransformSystem>();
            EcsWorld.AddSystem<FreezeMovementSystem>();
            EcsWorld.AddSystem<DeathAnimationSystem>();
            EcsWorld.AddSystem<SpawnerSystem>();
        }

        private void CreatePlayer(int entityId, MonoBehaviorProvider provider, Vector3 position)
        {
            EcsWorld.AddEntityComponent(entityId, new PositionComponent(position));
            EcsWorld.AddEntityComponent(entityId, new PlayerTagComponent());
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(8f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(10f));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(provider.Transform));
            EcsWorld.AddEntityComponent(entityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(entityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(entityId, new PlayerLookRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(provider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(100f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(5f, 3f));
        }

        private void CreateEnemy(int entityId, MonoBehaviorProvider playerProvider)
        {
            var enemyProvider = Instantiate(EnemyPrefab);

            EcsWorld.AddEntityComponent(entityId,
                new PositionComponent(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f))));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(1f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(10f));
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(enemyProvider.Transform));
            EcsWorld.AddEntityComponent(entityId, new EnemyTagComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new FollowComponent(playerProvider.Transform));
            EcsWorld.AddEntityComponent(entityId, new SeparationComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(enemyProvider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(50f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(2f, 5f));
            EcsWorld.AddEntityComponent(entityId, new FreezeMovementByDamageComponent(1.5f));
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
