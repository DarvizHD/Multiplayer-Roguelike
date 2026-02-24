using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components.Battle
{
    public class MeleeAttackSystem : BaseSystem
    {
        public MeleeAttackSystem()
        {
            RegisterRequiredComponent((typeof(PositionComponent)));
            RegisterRequiredComponent((typeof(MeleeAttackComponent)));
            RegisterRequiredComponent((typeof(AttackCooldownComponent)));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent = components[0] as PositionComponent;
            var meleeAttackComponent = components[1] as MeleeAttackComponent;
            var attackCooldownComponent = components[2] as AttackCooldownComponent;
            
            if (attackCooldownComponent.CurrentCooldown > 0)
            {
                return;
            }

            attackCooldownComponent.CurrentCooldown = attackCooldownComponent.Cooldown;

            var targets = ComponentManager.Query(typeof(PositionComponent), typeof(EnemyTagComponent));

            foreach (var (targetId, targetComponents) in targets)
            {
                var targetPositionComponent = targetComponents[0] as PositionComponent;

                var distance = Vector3.Distance(targetPositionComponent.Position, positionComponent.Position);
                
                if (distance < meleeAttackComponent.Range)
                {
                    if (!ComponentManager.TryGetComponent<PendingDamageEventComponent>(targetId, out var pendingDamageEventComponent))
                    {
                        ComponentManager.AddComponent(targetId, pendingDamageEventComponent = new PendingDamageEventComponent());
                    }
                    
                    pendingDamageEventComponent.TotalDamage += meleeAttackComponent.Damage;
                }
            }
        }
    }
}