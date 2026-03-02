using System.Collections.Generic;
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
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Shared.Models;
using Shared.Protocol;
using UnityEngine;

namespace Runtime
{
    public class GameSession
    {
        public ECSWorld EcsWorld { get; private set; }

        private ServerConnectionModel _serverConnectionModel;

        private readonly WorldSharedModel _worldSharedModel;

        private readonly PlayerSharedModel _playerSharedModel;

        private PlayerControls _playerControls;

        private readonly Dictionary<string, int> _characterEntities = new();

        public GameSession(WorldSharedModel worldSharedModel, PlayerSharedModel playerSharedModel, ServerConnectionModel serverConnectionModel)
        {
            _worldSharedModel = worldSharedModel;
            _playerSharedModel = playerSharedModel;
            _serverConnectionModel = serverConnectionModel;
        }

        public void Run()
        {
            EcsWorld = new ECSWorld();

            _playerControls = new PlayerControls();

            _playerControls.Enable();

            RegisterComponents();

            AddSystems();

            CreateCamera(6);

            //Setup();
        }

        public void CreateConnectedPlayers()
        {
            foreach (var character in _worldSharedModel.Characters.Models)
            {
                if (!_characterEntities.ContainsKey(character.Id))
                {
                    var entityId = _characterEntities.Count;

                    var controllable = _playerSharedModel.Nickname.Value == character.Id;

                    CreatePlayer(entityId, character, character.LastPosition.Value.ToUnityVector3(), controllable);

                    _characterEntities.Add(character.Id, entityId);
                }
            }
        }

        public void Update(float deltaTime)
        {
            EcsWorld?.Update(deltaTime);
        }

        private void Setup()
        {
            for (var i = 10; i < 100; i++)
            {
                CreateEnemy(i);
            }
        }

        private void CreateCamera(int entityId)
        {
            EcsWorld.AddEntityComponent(entityId, new CameraTargetComponent());
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(Camera.main.transform.parent.GetChild(2)));
        }

        private void CreatePlayer(int entityId, CharacterSharedModel characterSharedModel, Vector3 position, bool controllable)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Player");

            var provider = Object.Instantiate(prefab);

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
            EcsWorld.AddEntityComponent(entityId, new CharacterNetworkSyncComponent(characterSharedModel));

            if (controllable)
            {
                EcsWorld.AddEntityComponent(entityId, new PlayerInputComponent(_playerControls));
                EcsWorld.AddEntityComponent(entityId, new CharacterConnectionComponent(_serverConnectionModel));
            }
        }

        private void CreateEnemy(int entityId)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Enemy");

            var enemyProvider = Object.Instantiate(prefab);

            var randomPosition = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-10f, 10f));
            var speed = 1f;

            EcsWorld.AddEntityComponent(entityId, new PositionComponent(randomPosition));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(1f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(10f));
            EcsWorld.AddEntityComponent(entityId, new EnemyTagComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(enemyProvider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(50f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(2f, 5f));
            EcsWorld.AddEntityComponent(entityId, new FreezeMovementByDamageComponent(1.5f));
            EcsWorld.AddEntityComponent(entityId, new NavMeshAgentComponent(enemyProvider.Agent, randomPosition,  speed));
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
        }

        private void AddSystems()
        {
            EcsWorld.AddSystem<PlayerInputMovementSystem>();
            EcsWorld.AddSystem<PlayerLookRotationSystem>();
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<CharacterSharedSyncSystem>();
            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
            EcsWorld.AddSystem<CameraFocusSystem>();

            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<DrawCameraTransformSystem>();

            /*
            EcsWorld.AddSystem<DirectionRotationSystem>();
            EcsWorld.AddSystem<FollowSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<RegenerationSystem>();
            EcsWorld.AddSystem<InvulnerabilitySystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();
            EcsWorld.AddSystem<MeleeAttackAnimationSystem>();
            EcsWorld.AddSystem<AttackCooldownSystem>();
            EcsWorld.AddSystem<AttackSystem>();
            EcsWorld.AddSystem<DamageAnimationSystem>();
            EcsWorld.AddSystem<FreezeMovementByDamageSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<DeathSystem>();
            EcsWorld.AddSystem<AINavigationSystem>();
            EcsWorld.AddSystem<FreezeMovementSystem>();
            EcsWorld.AddSystem<DeathAnimationSystem>();*/
        }
    }

    public class ClientModel
    {
        public string Nickname;

        public string LobbyId;

        public bool IsRun;
    }

    public class EntryPoint : MonoBehaviour
    {
        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private GameSession _gameSession;
        private ClientModel _clientModel;

        private WorldSharedModel _worldSharedModel;
        private PlayerSharedModel _playerSharedModel;

        private ServerConnectionModel _serverConnectionModel;
        private ServerConnectionPresenter _serverConnectionPresenter;

        [SerializeField] private ClientUI _clientUI;

        private async void Start()
        {
            Application.runInBackground = true;

            _playerSharedModel = new PlayerSharedModel(string.Empty);
            _worldSharedModel = new WorldSharedModel(string.Empty);
            _clientModel = new ClientModel();

            Library.Initialize();

            _serverConnectionModel = new ServerConnectionModel();
            _serverConnectionPresenter = new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
            _serverConnectionPresenter.Enable();

            _serverConnectionModel.ConnectPlayer();
            await _serverConnectionModel.CompletePlayerConnectAwaiter;

            _gameSession = new GameSession(_worldSharedModel, _playerSharedModel, _serverConnectionModel);

            _serverConnectionModel.WorldPacketReceived += OnWorldPacketReceived;
            _serverConnectionModel.PlayerPacketReceived += OnPlayerPacketReceived;

            _clientUI.Construct(_clientModel, _serverConnectionModel, _gameSession);

            _worldSharedModel.IsRun.OnChange += RunSession;
        }

        private void FixedUpdate()
        {
            _gameSession?.Update(Time.fixedDeltaTime);
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }

        private void OnWorldPacketReceived(Packet packet)
        {
            var buffer = new byte[1024];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string id);

            _worldSharedModel.Read(protocol);
            _gameSession.CreateConnectedPlayers();
            Debug.Log($"Is Run {_worldSharedModel.IsRun}");
        }

        private void OnPlayerPacketReceived(Packet packet)
        {
            var buffer = new byte[1024];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string id);
            _playerSharedModel.Read(protocol);

            _clientModel.LobbyId = _playerSharedModel.Lobby.LobbyId.Value;
            _clientModel.Nickname = _playerSharedModel.Nickname.Value;
        }

        private void RunSession()
        {
            _gameSession.Run();

            Debug.Log("Session running.");
        }
    }
}
