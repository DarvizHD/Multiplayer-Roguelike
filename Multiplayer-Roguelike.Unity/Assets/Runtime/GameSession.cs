using System.Collections.Generic;
using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Movement.Freeze;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Components.Spawn;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems;
using Runtime.ECS.Systems.AI;
using Runtime.ECS.Systems.Battle;
using Runtime.ECS.Systems.Battle.MeleeAttack;
using Runtime.ECS.Systems.CameraFocus;
using Runtime.Ecs.Systems.Movement;
using Runtime.ECS.Systems.Movement;
using Runtime.ECS.Systems.Movement.Freeze;
using Runtime.ECS.Systems.Network;
using Runtime.ECS.Systems.Player;
using Runtime.ECS.Systems.Rotation;
using Runtime.ServerInteraction;
using Runtime.Tools;
using Shared.Commands;
using Shared.Models;
using UnityEngine;

namespace Runtime
{
    public class GameSession
    {
        public EcsWorld EcsWorld { get; private set; }

        private readonly ServerConnectionModel _serverConnectionModel;

        private readonly GameSessionSharedModel _gameSessionSharedModel;

        private readonly PlayerSharedModel _playerSharedModel;

        private PlayerControls _playerControls;

        private readonly Dictionary<string, int> _characterEntities = new();

        private bool IsHost => _playerSharedModel.Lobby.OwnerId.Value == _playerSharedModel.Nickname.Value;

        public GameSession(GameSessionSharedModel gameSessionSharedModel, PlayerSharedModel playerSharedModel,
            ServerConnectionModel serverConnectionModel)
        {
            _gameSessionSharedModel = gameSessionSharedModel;
            _playerSharedModel = playerSharedModel;
            _serverConnectionModel = serverConnectionModel;
        }

        public void Enable()
        {
            EcsWorld = new EcsWorld();

            RegisterComponents();

            _playerControls = new PlayerControls();

            _playerControls.Enable();

            _gameSessionSharedModel.Characters.Added += OnCharacterAdded;

            _gameSessionSharedModel.NPCs.Added += OnNpcAdded;
        }

        public void Disable()
        {
            _gameSessionSharedModel.Characters.Added -= OnCharacterAdded;

            _gameSessionSharedModel.NPCs.Added -= OnNpcAdded;
        }

        public void Run()
        {
            AddSystems();

            CreateCamera(6);

            SpawnNpc();
        }

        private void OnCharacterAdded(CharacterSharedModel characterSharedModel)
        {
            Debug.Log("Worked");

            var entityId = (ushort) _characterEntities.Count;

            var controllable = _playerSharedModel.Nickname.Value == characterSharedModel.Id;

            CreatePlayer(entityId, characterSharedModel, characterSharedModel.Position.Value.ToUnityVector3(),
                controllable);

            _characterEntities.Add(characterSharedModel.Id, entityId);
        }

        private void OnNpcAdded(NpcSharedModel npcSharedModel)
        {
            var npcId = ushort.Parse(npcSharedModel.Id);
            CreateEnemy(npcId, npcSharedModel.LastPosition.Value.ToUnityVector3());
        }

        public void Update(float deltaTime)
        {
            EcsWorld?.Update(deltaTime);
        }

        private void SpawnNpc()
        {
            if (!IsHost)
            {
                Debug.Log("I'm not host");

                return;
            }

            var spawnNpcCommand = new SpawnNpcCommand(_playerSharedModel.Lobby.LobbyId.Value, 10);

            spawnNpcCommand.Write(_serverConnectionModel.PlayerPeer);
        }

        private void CreateCamera(ushort entityId)
        {
            EcsWorld.AddEntityComponent(entityId, new CameraTargetComponent());
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(Camera.main?.transform.parent.GetChild(2)));
        }

        private void CreatePlayer(ushort entityId, CharacterSharedModel characterSharedModel, Vector3 position,
            bool controllable)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Player");

            var provider = Object.Instantiate(prefab);

            EcsWorld.AddEntityComponent(entityId, new PositionComponent(position));
            EcsWorld.AddEntityComponent(entityId, new PlayerTagComponent());
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(8f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(360f));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(provider.Transform));
            EcsWorld.AddEntityComponent(entityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(entityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(entityId, new PlayerLookRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(provider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(100f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(5f, 3f));
            EcsWorld.AddEntityComponent(entityId, new CharacterNetworkSyncComponent(characterSharedModel));

            if (controllable)
            {
                EcsWorld.AddEntityComponent(entityId, new PlayerInputComponent(_playerControls));
                EcsWorld.AddEntityComponent(entityId, new CharacterConnectionComponent(_serverConnectionModel));
                EcsWorld.AddEntityComponent(entityId, new LocalControllableTag());
                EcsWorld.AddEntityComponent(entityId, new RigidbodyComponent(provider.Rigidbody));
            }
            else
            {
                EcsWorld.AddEntityComponent(entityId, new NetworkControllableTag());
                EcsWorld.AddEntityComponent(entityId, new PositionInterpolationComponent(Vector3.zero, Vector3.zero));
            }
        }

        private void CreateEnemy(ushort entityId, Vector3 spawnPosition)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Enemy");

            var enemyProvider = Object.Instantiate(prefab);

            var speed = 1f;

            EcsWorld.AddEntityComponent(entityId, new PositionComponent(spawnPosition));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(1f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(360f));
            EcsWorld.AddEntityComponent(entityId, new EnemyTagComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(enemyProvider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(50f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(2f, 5f));
            EcsWorld.AddEntityComponent(entityId, new FreezeMovementByDamageComponent(1.5f));
            EcsWorld.AddEntityComponent(entityId, new NavMeshAgentComponent(enemyProvider.Agent, spawnPosition, speed));

            EcsWorld.AddEntityComponent(entityId, new LocalControllableTag());
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
            EcsWorld.RegisterComponent<NavMeshAgentComponent>();

            EcsWorld.RegisterComponent<CharacterConnectionComponent>();
            EcsWorld.RegisterComponent<CharacterNetworkSyncComponent>();

            EcsWorld.RegisterComponent<LocalControllableTag>();
            EcsWorld.RegisterComponent<NetworkControllableTag>();
            EcsWorld.RegisterComponent<PositionInterpolationComponent>();
            EcsWorld.RegisterComponent<RigidbodyComponent>();
        }

        private void AddSystems()
        {
            EcsWorld.AddSystem<CharacterPositionSyncSystem>();
            EcsWorld.AddSystem<CharacterRotationSyncSystem>();

            EcsWorld.AddSystem<PlayerInputMovementSystem>();
            EcsWorld.AddSystem<PlayerLookRotationSystem>();

            EcsWorld.AddSystem<PlayerMovementSystem>();
            EcsWorld.AddSystem<PositionInterpolationSystem>();

            EcsWorld.AddSystem<CharacterPositionSendSystem>();
            EcsWorld.AddSystem<CharacterRotationSendSystem>();

            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
            EcsWorld.AddSystem<CameraFocusSystem>();

            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<DrawCameraTransformSystem>();

            EcsWorld.AddSystem<AINavigationSystem>();
            EcsWorld.AddSystem<AIPositionSyncSystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();

            EcsWorld.AddSystem<AttackCooldownSystem>();

            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<MeleeAttackAnimationSystem>();

            EcsWorld.AddSystem<AttackSystem>();
            EcsWorld.AddSystem<FreezeMovementByDamageSystem>();
            EcsWorld.AddSystem<FreezeMovementSystem>();

            EcsWorld.AddSystem<DamageAnimationSystem>();
            EcsWorld.AddSystem<DamageSystem>();

            /*
            EcsWorld.AddSystem<RegenerationSystem>();
            EcsWorld.AddSystem<InvulnerabilitySystem>();
            EcsWorld.AddSystem<DeathSystem>();
            EcsWorld.AddSystem<DeathAnimationSystem>();*/
        }
    }
}
