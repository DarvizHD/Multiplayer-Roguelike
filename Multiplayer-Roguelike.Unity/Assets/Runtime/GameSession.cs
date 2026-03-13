using System.Collections.Generic;
using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Movement.Freeze;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Components.Spawn;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems;
using Runtime.Ecs.Systems.AI;
using Runtime.Ecs.Systems.Battle;
using Runtime.Ecs.Systems.Battle.MeleeAttack;
using Runtime.Ecs.Systems.Battle.RangeAttack;
using Runtime.Ecs.Systems.CameraFocus;
using Runtime.Ecs.Systems.Movement;
using Runtime.ECS.Systems.Particles;
using Runtime.Ecs.Systems.Player;
using Runtime.Ecs.Systems.Player.Network;
using Runtime.Ecs.Systems.Player.Rotation;
using Runtime.Ecs.Systems.Sound;
using Runtime.Ecs.Systems.UI;
using Runtime.Ecs.Systems.Weapons;
using Runtime.ServerInteraction;
using Runtime.Sound;
using Runtime.Tools;
using Runtime.UI;
using Runtime.UI.HUD;
using Shared.Commands;
using Shared.Models.Enemy;
using Shared.Models.GameSession;
using Shared.Models.Player;
using UnityEngine;

namespace Runtime
{
    public class GameSession
    {
        public EcsWorld EcsWorld { get; private set; }

        private readonly ServerConnectionModel _serverConnectionModel;
        private readonly GameSessionSharedModel _gameSessionSharedModel;
        private readonly PlayerSharedModel _playerSharedModel;
        private readonly Dictionary<string, int> _characterEntities = new();
        private AudioClip[] _zombieVoiceClips;

        private PlayerControls _playerControls;
        private WorldViewDescription _worldViewDescription;
        private readonly UIHudView _hudView;

        private bool IsHost => _playerSharedModel.Lobby.OwnerId.Value == _playerSharedModel.Nickname.Value;

        public GameSession(GameSessionSharedModel gameSessionSharedModel, PlayerSharedModel playerSharedModel,
            ServerConnectionModel serverConnectionModel, UIHudView hudView, WorldViewDescription worldViewDescription)
        {
            _gameSessionSharedModel = gameSessionSharedModel;
            _playerSharedModel = playerSharedModel;
            _serverConnectionModel = serverConnectionModel;
            _hudView = hudView;
            _worldViewDescription = worldViewDescription;
        }

        public void Enable()
        {
            EcsWorld = new EcsWorld();

            RegisterComponents();

            _playerControls = new PlayerControls();

            _playerControls.Enable();

            _gameSessionSharedModel.Characters.Added += OnCharacterAdded;

            _gameSessionSharedModel.Enemies.Added += OnNpcAdded;

            _zombieVoiceClips = Resources.LoadAll<AudioClip>("Audio/SFX/Enemies/ZombieVoices");
        }

        public void Disable()
        {
            _gameSessionSharedModel.Characters.Added -= OnCharacterAdded;

            _gameSessionSharedModel.Enemies.Added -= OnNpcAdded;
        }

        public void Run()
        {
            AddSystems();

            CreateCamera(6);

            SpawnNpc();
        }

        private void OnCharacterAdded(CharacterSharedModel characterSharedModel)
        {
            var entityId = EcsWorld.CreateEntity();

            var controllable = _playerSharedModel.Nickname.Value == characterSharedModel.Id;

            CreatePlayer(entityId, characterSharedModel, characterSharedModel.Position.Value.ToUnityVector3(),
                controllable);

            _characterEntities.Add(characterSharedModel.Id, entityId);
        }

        private void OnNpcAdded(EnemySharedModel enemySharedModel)
        {
            var npcId = ushort.Parse(enemySharedModel.Id);
            CreateEnemy(npcId, enemySharedModel.Position.Value.ToUnityVector3(), enemySharedModel);
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

            var spawnNpcCommand = new SpawnNpcCommand(_playerSharedModel.Lobby.LobbyId.Value, _playerSharedModel.Nickname.Value, 10);

            spawnNpcCommand.Write(_serverConnectionModel.PlayerPeer);
        }

        private void CreateCamera(ushort entityId)
        {
            EcsWorld.AddEntityComponent(entityId, new CameraTargetComponent());
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(Camera.main?.transform.parent.GetChild(2)));
        }

        private ushort CreateMeleeWeapon()
        {
            var entityId = EcsWorld.CreateEntity();
            var clip = Resources.Load<AudioClip>(AudioResourcesConstants.Weapon.BaseballBatHit);
            EcsWorld.AddEntityComponent(entityId, new MeleeAttackComponent(25f, 2f, clip));
            EcsWorld.AddEntityComponent(entityId, new AttackCooldownComponent(2f));
            return entityId;
        }

        private ushort CreateRangedWeapon()
        {
            var entityId = EcsWorld.CreateEntity();
            var shootClip = Resources.Load<AudioClip>(AudioResourcesConstants.Weapon.PistolShot);
            var reloadClip = Resources.Load<AudioClip>(AudioResourcesConstants.Weapon.WeaponReload);
            EcsWorld.AddEntityComponent(entityId, new RangedWeaponComponent(50f, 2f, 2f, shootClip, reloadClip));
            EcsWorld.AddEntityComponent(entityId, new AmmoComponent(7, 30));
            EcsWorld.AddEntityComponent(entityId, new AttackCooldownComponent(0.75f));
            return entityId;
        }

        private void CreatePlayer(ushort entityId, CharacterSharedModel characterSharedModel, Vector3 position,
            bool controllable)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Player");
            var provider = Object.Instantiate(prefab);

            var playerHitClip = Resources.Load<AudioClip>(AudioResourcesConstants.Player.PlayerTakeDamage);

            EcsWorld.AddEntityComponent(entityId, new NameComponent(characterSharedModel.Id));
            EcsWorld.AddEntityComponent(entityId, new PositionComponent(position));
            EcsWorld.AddEntityComponent(entityId, new PlayerTagComponent());
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(8f));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(360f));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(entityId, new TransformComponent(provider.Transform));
            EcsWorld.AddEntityComponent(entityId, new PlayerLookRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(provider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(100f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(5f, 3f));
            EcsWorld.AddEntityComponent(entityId, new CharacterNetworkSyncComponent(characterSharedModel));
            EcsWorld.AddEntityComponent(entityId, new WeaponProviderComponent(provider.WeaponProvider));
            EcsWorld.AddEntityComponent(entityId, new SfxContainerComponent(provider.SfxContainer));
            EcsWorld.AddEntityComponent(entityId, new AliveTagComponent());

            var meleeId = CreateMeleeWeapon();
            var rangedId = CreateRangedWeapon();

            EcsWorld.AddEntityComponent(entityId, new WeaponSlotsComponent(new[] { meleeId, rangedId }));
            EcsWorld.AddEntityComponent(entityId, new CurrentWeaponComponent(meleeId));

            EcsWorld.AddEntityComponent(entityId, new ShootParticlePointComponent(provider.ShootPoint));
            EcsWorld.AddEntityComponent(entityId, new HitSoundComponent(playerHitClip));

            if (controllable)
            {
                EcsWorld.AddEntityComponent(entityId, new PlayerInputComponent(_playerControls));
                EcsWorld.AddEntityComponent(entityId, new CharacterConnectionComponent(_serverConnectionModel));
                EcsWorld.AddEntityComponent(entityId, new LocalControllableTag());
                EcsWorld.AddEntityComponent(entityId, new RigidbodyComponent(provider.Rigidbody, position));
                EcsWorld.AddEntityComponent(entityId, new CursorWorldPositionComponent());
            }
            else
            {
                EcsWorld.AddEntityComponent(entityId, new NetworkControllableTag());
                EcsWorld.AddEntityComponent(entityId, new PositionInterpolationComponent(Vector3.zero, Vector3.zero));
                EcsWorld.AddEntityComponent(entityId, new CharacterNetworkEventComponent(string.Empty));
            }
        }

        private void CreateEnemy(ushort entityId, Vector3 spawnPosition, EnemySharedModel enemySharedModel)
        {
            var prefab = Resources.Load<MonoBehaviorProvider>("Enemy");
            var enemyProvider = Object.Instantiate(prefab);

            var enemyHitClip = Resources.Load<AudioClip>(AudioResourcesConstants.Enemies.ZombieTakeDamage);

            var speed = 1f;

            EcsWorld.AddEntityComponent(entityId, new NameComponent($"Zombie {entityId}"));
            EcsWorld.AddEntityComponent(entityId, new PositionComponent(spawnPosition));
            EcsWorld.AddEntityComponent(entityId, new RotationComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(entityId, new MoveSpeedComponent(speed));
            EcsWorld.AddEntityComponent(entityId, new RotationSpeedComponent(360f));
            EcsWorld.AddEntityComponent(entityId, new EnemyTagComponent());
            EcsWorld.AddEntityComponent(entityId, new DirectionRotationTagComponent());
            EcsWorld.AddEntityComponent(entityId, new AttackCooldownComponent(2f));
            EcsWorld.AddEntityComponent(entityId, new AnimatorComponent(enemyProvider.Animator));
            EcsWorld.AddEntityComponent(entityId, new HealthComponent(100f));
            EcsWorld.AddEntityComponent(entityId, new RegenerationComponent(2f, 5f));
            EcsWorld.AddEntityComponent(entityId, new FreezeMovementByDamageComponent(1.5f));
            EcsWorld.AddEntityComponent(entityId, new AliveTagComponent());
            EcsWorld.AddEntityComponent(entityId, new EnemyNetworkSyncComponent(enemySharedModel));
            EcsWorld.AddEntityComponent(entityId, new PositionInterpolationComponent(Vector3.zero, Vector3.zero));
            EcsWorld.AddEntityComponent(entityId, new NavMeshAgentComponent(enemyProvider.Agent, spawnPosition, speed));
            EcsWorld.AddEntityComponent(entityId, new SfxContainerComponent(enemyProvider.SfxContainer));

            EcsWorld.AddEntityComponent(entityId, new LocalControllableTag());
            EcsWorld.AddEntityComponent(entityId, new RagdollComponent(enemyProvider.RagdollProvider));

            var voiceClip = _zombieVoiceClips[Random.Range(0, _zombieVoiceClips.Length)];
            var voiceDelay = Random.Range(0f, 3f);
            EcsWorld.AddEntityComponent(entityId, new ZombieVoiceComponent(enemyProvider.LoopAudioSource, voiceClip, voiceDelay));
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
            EcsWorld.RegisterComponent<EnemyNetworkSyncComponent>();

            EcsWorld.RegisterComponent<CharacterConnectionComponent>();
            EcsWorld.RegisterComponent<CharacterNetworkSyncComponent>();

            EcsWorld.RegisterComponent<LocalControllableTag>();
            EcsWorld.RegisterComponent<NetworkControllableTag>();
            EcsWorld.RegisterComponent<PositionInterpolationComponent>();
            EcsWorld.RegisterComponent<RigidbodyComponent>();
            EcsWorld.RegisterComponent<AliveTagComponent>();
            EcsWorld.RegisterComponent<DeathEventComponent>();

            EcsWorld.RegisterComponent<RagdollComponent>();
            EcsWorld.RegisterComponent<NameComponent>();

            EcsWorld.RegisterComponent<RangedWeaponComponent>();
            EcsWorld.RegisterComponent<AmmoComponent>();
            EcsWorld.RegisterComponent<WeaponSlotsComponent>();
            EcsWorld.RegisterComponent<CurrentWeaponComponent>();
            EcsWorld.RegisterComponent<SwitchWeaponEventComponent>();
            EcsWorld.RegisterComponent<ReloadEventComponent>();
            EcsWorld.RegisterComponent<CursorWorldPositionComponent>();

            EcsWorld.RegisterComponent<WeaponProviderComponent>();

            EcsWorld.RegisterComponent<DamageAnimationEventComponent>();
            EcsWorld.RegisterComponent<AttackAnimationEventComponent>();
            EcsWorld.RegisterComponent<CharacterNetworkEventComponent>();
            EcsWorld.RegisterComponent<SfxContainerComponent>();
            EcsWorld.RegisterComponent<PlaySoundEventComponent>();
            EcsWorld.RegisterComponent<HitSoundComponent>();
            EcsWorld.RegisterComponent<ZombieVoiceComponent>();

            EcsWorld.RegisterComponent<ShootParticlePointComponent>();
            EcsWorld.RegisterComponent<DamageParticleEventComponent>();
            EcsWorld.RegisterComponent<DeathParticleEventComponent>();
            EcsWorld.RegisterComponent<ShootParticleEventComponent>();
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

            EcsWorld.AddSystem<AINavigationSyncSystem>();
            EcsWorld.AddSystem<AIPositionSyncSystem>();
            EcsWorld.AddSystem<EnemyMovementAnimationSystem>();

            EcsWorld.AddSystem<CursorWorldPositionSystem>();

            EcsWorld.AddSystem<WeaponSwitchInputSystem>();
            EcsWorld.AddSystem<WeaponSwitchSyncSystem>();

            EcsWorld.AddSystem<WeaponSwitchNetworkSystem>();

            EcsWorld.AddSystem<DrawWeaponSwitcherSystem>();
            EcsWorld.AddSystem<WeaponAnimationSwitcherSystem>();
            EcsWorld.AddSystem<WeaponSwitchHandlerSystem>();

            EcsWorld.AddSystem<AttackCooldownSystem>();

            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<MeleeAttackAnimationSystem>();
            EcsWorld.AddSystem<RangedAttackSystem>();
            EcsWorld.AddSystem<ReloadSystem>();

            EcsWorld.AddSystem<CharacterAttackSystem>();

            EcsWorld.AddSystem<AIHealthSync>();
            EcsWorld.AddSystem<CharacterHealthSyncSystem>();
            EcsWorld.AddSystem<AIDeathSystem>();
            EcsWorld.AddSystem<ShootParticleSystem>(new ShootParticleSystem(new PinnedParticlePool(_worldViewDescription.ShootParticle)));
            EcsWorld.AddSystem<DamageParticleSystem>(new DamageParticleSystem(new PositionalParticlePool(_worldViewDescription.DamageParticle)));
            EcsWorld.AddSystem<DeathParticleSystem>(new DeathParticleSystem(new PositionalParticlePool(_worldViewDescription.DeathParticle)));
            EcsWorld.AddSystem<DamageAnimationSystem>();

            EcsWorld.AddSystem<CharacterAnimationSyncSystem>();

            EcsWorld.AddSystem(new PlaySoundSystem());
            EcsWorld.AddSystem<ZombieVoiceSystem>();

            EcsWorld.AddSystem(new UIDrawNameSystem(_hudView));
            EcsWorld.AddSystem(new UIDrawHealthSystem(_hudView));
            EcsWorld.AddSystem(new UIDrawSwitchWeapon(_hudView));
            EcsWorld.AddSystem(new UIDrawAmmo(_hudView));
            EcsWorld.AddSystem(new UIDrawTeammates(_hudView));
            EcsWorld.AddSystem(new UIDrawCurrentPlayerHealth(_hudView));

            /*
            EcsWorld.AddSystem<FreezeMovementByDamageSystem>();
            EcsWorld.AddSystem<FreezeMovementSystem>();
            EcsWorld.AddSystem<RegenerationSystem>();
            EcsWorld.AddSystem<InvulnerabilitySystem>();
            EcsWorld.AddSystem<DeathAnimationSystem>();*/
        }
    }
}
