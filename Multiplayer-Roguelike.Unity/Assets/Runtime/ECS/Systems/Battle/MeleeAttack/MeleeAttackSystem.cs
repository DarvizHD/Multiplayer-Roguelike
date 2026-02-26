using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Tags;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle.MeleeAttack
{
    public class MeleeAttackSystem : BaseSystem
    {
        public MeleeAttackSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
            RegisterRequiredComponent(typeof(MeleeAttackComponent));
            RegisterRequiredComponent(typeof(AttackCooldownComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            if (ComponentManager.HasComponent<DeathComponent>(id))
                return;
        
            var positionComponent = components[0] as PositionComponent;
            var rotationComponent = components[1] as RotationComponent;
            var meleeAttackComponent = components[2] as MeleeAttackComponent;
            var attackCooldownComponent = components[3] as AttackCooldownComponent;

            if (attackCooldownComponent.CurrentCooldown > 0)
            {
                return;
            }

            var attackDirection = Quaternion.Euler(0f, rotationComponent.Angle, 0f) * Vector3.forward;
            attackDirection.y = 0;

            var targets = ComponentManager.Query(typeof(PositionComponent), typeof(EnemyTagComponent));

            foreach (var (targetId, targetComponents) in targets)
            {
                if (ComponentManager.HasComponent<DeathComponent>(targetId))
                    continue;
            
                var targetPositionComponent = targetComponents[0] as PositionComponent;

                var distance = Vector3.Distance(targetPositionComponent.Position, positionComponent.Position);

                if (distance >= meleeAttackComponent.Range) continue;

                var toTarget = targetPositionComponent.Position - positionComponent.Position;
                toTarget.y = 0;
                toTarget.Normalize();

                var angle = Vector3.Angle(attackDirection, toTarget);

                if (angle > meleeAttackComponent.Angle * 0.5f) continue;

                if (!ComponentManager.HasComponent<AttackEventComponent>(id))
                {
                    attackCooldownComponent.CurrentCooldown = attackCooldownComponent.Cooldown;
                    
                    ComponentManager.AddComponent(id, new AttackEventComponent(targetId, meleeAttackComponent.Damage));
                }
            }
        }
    }
}