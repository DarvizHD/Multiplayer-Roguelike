using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
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
            var positionComponent = components[0] as PositionComponent;
            var rotationComponent = components[1] as RotationComponent;
            var meleeAttackComponent = components[2] as MeleeAttackComponent;
            var attackCooldownComponent = components[3] as AttackCooldownComponent;

            if (attackCooldownComponent.CurrentCooldown > 0)
            {
                return;
            }

            attackCooldownComponent.CurrentCooldown = attackCooldownComponent.Cooldown;

            var attackDirection = Quaternion.Euler(0f, rotationComponent.Angle, 0f) * Vector3.forward;
            attackDirection.y = 0;

            var targets = ComponentManager.Query(typeof(PositionComponent), typeof(EnemyTagComponent));

            foreach (var (targetId, targetComponents) in targets)
            {
                var targetPositionComponent = targetComponents[0] as PositionComponent;

                var distance = Vector3.Distance(targetPositionComponent.Position, positionComponent.Position);

                if (distance >= meleeAttackComponent.Range) continue;

                var toTarget = targetPositionComponent.Position - positionComponent.Position;
                toTarget.y = 0;
                toTarget.Normalize();

                var angle = Vector3.Angle(attackDirection, toTarget);

                if (angle > meleeAttackComponent.Angle * 0.5f) continue;

                if (!ComponentManager.TryGetComponent<AttackEventComponent>(id, out var attackEventComponent))
                {
                    ComponentManager.AddComponent(id, new AttackEventComponent(targetId, meleeAttackComponent.Damage));
                }
            }
        }
    }
}