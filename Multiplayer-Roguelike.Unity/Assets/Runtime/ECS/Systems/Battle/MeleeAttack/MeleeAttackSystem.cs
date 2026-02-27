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

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, positionComponent, rotationComponent, meleeAttackComponent, attackCooldownComponent)
                     in ComponentManager.Query<PositionComponent, RotationComponent, MeleeAttackComponent, AttackCooldownComponent>())
            {
                if (ComponentManager.HasComponent<DeathTagComponent>(entityId))
                    return;

                if (attackCooldownComponent.CurrentCooldown > 0)
                {
                    return;
                }

                var attackDirection = Quaternion.Euler(0f, rotationComponent.Angle, 0f) * Vector3.forward;
                attackDirection.y = 0;

                var targets = ComponentManager.Query<PositionComponent, EnemyTagComponent>();

                foreach (var (targetId, targetPositionComponent, enemyTagComponent) in targets)
                {
                    if (ComponentManager.HasComponent<DeathTagComponent>(targetId))
                        if (targetId == entityId)
                            continue;

                    if (ComponentManager.HasComponent<DeathAnimationComponent>(targetId))
                        continue;

                    var distance = Vector3.Distance(targetPositionComponent.Position, positionComponent.Position);

                    if (distance >= meleeAttackComponent.Range) continue;

                    var toTarget = targetPositionComponent.Position - positionComponent.Position;
                    toTarget.y = 0;
                    toTarget.Normalize();

                    var angle = Vector3.Angle(attackDirection, toTarget);

                    if (angle > meleeAttackComponent.Angle * 0.5f) continue;

                    if (!ComponentManager.HasComponent<AttackEventComponent>(entityId))
                    {
                        attackCooldownComponent.CurrentCooldown = attackCooldownComponent.Cooldown;

                        ComponentManager.AddComponent(entityId, new AttackEventComponent(targetId, meleeAttackComponent.Damage));
                    }
                }
            }
        }
    }
}
