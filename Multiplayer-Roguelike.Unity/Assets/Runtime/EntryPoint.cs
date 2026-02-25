using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.ECS.Systems.Battle;
using Runtime.ECS.Systems.Movement;
using Runtime.ECS.Systems.Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECSWorld EcsWorld { get; } = new ();
        public MonoBehaviorProvider PlayerPrefab;
        public GameObject EnemyPrefab;
        private PlayerControls _playerControls;
        
        private void Start()
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
            EcsWorld.RegisterComponent<TransformComponent>();
            EcsWorld.RegisterComponent<EnemyTagComponent>();
            EcsWorld.RegisterComponent<PendingDamageEventComponent>();
            EcsWorld.RegisterComponent<SeparationComponent>();
            EcsWorld.RegisterComponent<PlayerInputComponent>();
            EcsWorld.RegisterComponent<FollowComponent>();
            EcsWorld.RegisterComponent<RotationComponent>();
            EcsWorld.RegisterComponent<AnimatorComponent>();
            
            var playerEntityId = 0;
            
            var playerProvider = Instantiate(PlayerPrefab);
            
            EcsWorld.AddEntityComponent(playerEntityId, new PositionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new DirectionComponent(Random.insideUnitSphere.normalized));
            EcsWorld.AddEntityComponent(playerEntityId, new TransformComponent(playerProvider.Transform));
            EcsWorld.AddEntityComponent(playerEntityId, new SpeedComponent(8f));
            EcsWorld.AddEntityComponent(playerEntityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(playerEntityId, new MeleeAttackComponent(2f, 10f));
            EcsWorld.AddEntityComponent(playerEntityId, new RotationComponent(Quaternion.identity));
            EcsWorld.AddEntityComponent(playerEntityId, new PlayerInputComponent(_playerControls));
            EcsWorld.AddEntityComponent(playerEntityId, new SeparationComponent());
            EcsWorld.AddEntityComponent(playerEntityId, new AnimatorComponent(playerProvider.Animator));
            
            var enemyId = 1;

            EcsWorld.AddEntityComponent(enemyId, new PositionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(enemyId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(enemyId, new SpeedComponent(5f));
            EcsWorld.AddEntityComponent(enemyId, new TransformComponent(Instantiate(EnemyPrefab).transform));
            EcsWorld.AddEntityComponent(enemyId, new EnemyTagComponent());
            EcsWorld.AddEntityComponent(enemyId, new RotationComponent(Quaternion.identity));
            EcsWorld.AddEntityComponent(enemyId, new FollowComponent(playerProvider.Transform));
            EcsWorld.AddEntityComponent(enemyId, new SeparationComponent());
            
            EcsWorld.AddSystem<PlayerInputSystem>();
            EcsWorld.AddSystem<FollowSystem>();
            EcsWorld.AddSystem<SeparationSystem>();
            EcsWorld.AddSystem<RotationSystem>();
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<PatrolSystem>();
            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<AttackCooldownSystem>();
            EcsWorld.AddSystem<PlayerMovementAnimationSystem>();
        }
        
        private void Update()
        {
            EcsWorld.Update(Time.deltaTime);
        }
    }
}