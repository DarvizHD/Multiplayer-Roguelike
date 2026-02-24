using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECSWorld EcsWorld { get; } = new ();

        private void Start()
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
            
            var playerEntityId = 0;
            
            EcsWorld.AddEntityComponent(playerEntityId, new PositionComponent(Vector3.zero));
            EcsWorld.AddEntityComponent(playerEntityId, new DirectionComponent(Random.insideUnitSphere.normalized));
            EcsWorld.AddEntityComponent(playerEntityId, new TransformComponent(GameObject.CreatePrimitive(PrimitiveType.Sphere).transform));
            EcsWorld.AddEntityComponent(playerEntityId, new AttackCooldownComponent(3f));
            EcsWorld.AddEntityComponent(playerEntityId, new MeleeAttackComponent(2f, 10f));
            
            var enemyId = 1;

            EcsWorld.AddEntityComponent(enemyId, new PositionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(enemyId, new DirectionComponent(Vector3.forward));
            EcsWorld.AddEntityComponent(enemyId, new SpeedComponent(0.1f));
            EcsWorld.AddEntityComponent(enemyId, new TransformComponent(GameObject.CreatePrimitive(PrimitiveType.Sphere).transform));
            EcsWorld.AddEntityComponent(enemyId, new EnemyTagComponent());

            
            EcsWorld.AddSystem<MovementSystem>();
            EcsWorld.AddSystem<PatrolSystem>();
            EcsWorld.AddSystem<DrawTransformSystem>();
            EcsWorld.AddSystem<DamageSystem>();
            EcsWorld.AddSystem<MeleeAttackSystem>();
            EcsWorld.AddSystem<AttackCooldownSystem>();
        }
        
        private void Update()
        {
            EcsWorld.Update(Time.deltaTime);
        }
    }
}