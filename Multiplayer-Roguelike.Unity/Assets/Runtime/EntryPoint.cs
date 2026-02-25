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
        public GameObject PlayerPrefab;
        public GameObject EnemyPrefab;
        private Transform _playerTransform;
        
        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private async void Start()
        {
            EcsWorld.RegisterComponent<PositionComponent>();
            EcsWorld.RegisterComponent<VelocityComponent>();
            EcsWorld.RegisterComponent<SpeedComponent>();
            EcsWorld.RegisterComponent<DirectionComponent>();
            EcsWorld.RegisterComponent<AttackCooldownComponent>();
            EcsWorld.RegisterComponent<MeleeAttackComponent>();
            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<EnemyTagComponent>();
            EcsWorld.RegisterComponent<PendingDamageEventComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();
            EcsWorld.RegisterComponent<SeparationComponent>();
            EcsWorld.RegisterComponent<PlayerComponent>();
            EcsWorld.RegisterComponent<RotationComponent>();
            
            var playerEntityId = 0;
            
            _playerTransform = Instantiate(PlayerPrefab).transform;
            
            EcsWorld.AddEntityComponent(playerEntityId, new PositionComponent(Vector3.up));
            EcsWorld.AddEntityComponent(playerEntityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new TransformComponent(_playerTransform));
            EcsWorld.AddEntityComponent(playerEntityId, new SpeedComponent(8f));
            EcsWorld.AddEntityComponent(playerEntityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(playerEntityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(playerEntityId, new RotationComponent(Quaternion.identity));
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerComponent());
            EcsWorld.AddEntityComponent(playerEntityId, new SeparationComponent());

            for (var i = 1; i < 21; i++)
            {
                EcsWorld.AddEntityComponent(i, new PositionComponent(new Vector3(Random.Range(-15f, 15f), 1f, Random.Range(-15f, 15f))));
                EcsWorld.AddEntityComponent(i, new DirectionComponent(Vector3.zero));
                EcsWorld.AddEntityComponent(i, new SpeedComponent(5f));
                EcsWorld.AddEntityComponent(i, new TransformComponent(Instantiate(EnemyPrefab).transform));
                EcsWorld.AddEntityComponent(i, new EnemyTagComponent());
                EcsWorld.AddEntityComponent(i, new RotationComponent(Quaternion.identity));
                EcsWorld.AddEntityComponent(i, new FollowComponent(_playerTransform));
                EcsWorld.AddEntityComponent(i, new SeparationComponent());
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