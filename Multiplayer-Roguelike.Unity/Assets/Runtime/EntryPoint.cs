using ENet;
using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.ECS.Systems.Battle;
using Runtime.ECS.Systems.Movement;
using Runtime.ECS.Systems.Runtime.ECS.Systems;
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
            EcsWorld.RegisterComponent<VelocityComponent>();
            EcsWorld.RegisterComponent<SpeedComponent>();
            EcsWorld.RegisterComponent<DirectionComponent>();
            EcsWorld.RegisterComponent<AttackCooldownComponent>();
            EcsWorld.RegisterComponent<MeleeAttackComponent>();
            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<EnemyTagComponent>();
            EcsWorld.RegisterComponent<PendingDamageEventComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();
            EcsWorld.RegisterComponent<SeparationComponent>();
            EcsWorld.RegisterComponent<PlayerInputComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();
            EcsWorld.RegisterComponent<RotationComponent>();
            EcsWorld.RegisterComponent<AnimatorComponent>();
            
            var playerEntityId = 0;
            
            var playerProvider = Instantiate(PlayerPrefab);
            
            EcsWorld.AddEntityComponent(playerEntityId, new PositionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new TransformComponent((playerProvider.Transform)));
            EcsWorld.AddEntityComponent(playerEntityId, new SpeedComponent(8f));
            EcsWorld.AddEntityComponent(playerEntityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(playerEntityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(playerEntityId, new RotationComponent(Quaternion.identity));
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerInputComponent(_playerControls));
            EcsWorld.AddEntityComponent(playerEntityId, new AnimatorComponent(playerProvider.Animator));

            for (var i = 1; i < 21; i++)
            {
              var enemyId = i;

              var enemyProvider = Instantiate(EnemyPrefab);

              EcsWorld.AddEntityComponent(enemyId, new PositionComponent(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f))));
              EcsWorld.AddEntityComponent(enemyId, new DirectionComponent(Vector3.forward));
              EcsWorld.AddEntityComponent(enemyId, new SpeedComponent(1f));
              EcsWorld.AddEntityComponent(enemyId, new TransformComponent(enemyProvider.Transform));
              EcsWorld.AddEntityComponent(enemyId, new EnemyTagComponent());
              EcsWorld.AddEntityComponent(enemyId, new RotationComponent(Quaternion.identity));
              EcsWorld.AddEntityComponent(enemyId, new FollowComponent(playerProvider.Transform));
              EcsWorld.AddEntityComponent(enemyId, new SeparationComponent());
              EcsWorld.AddEntityComponent(enemyId, new AnimatorComponent(enemyProvider.Animator));
            }
            
            EcsWorld.AddSystem<PlayerInputSystem>();
            EcsWorld.AddSystem<FollowSystem>();
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<SeparationSystem>();
            EcsWorld.AddSystem<RotationSystem>();
            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<AttackCooldownSystem>();
            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();
            
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