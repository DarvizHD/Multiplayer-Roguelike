using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Health;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class DamageSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, pendingDamageEventComponent, healthComponent)
                     in ComponentManager.Query<PendingDamageEventComponent, HealthComponent>())
            {
                if (healthComponent.CurrentHealth <= 0 || ComponentManager.HasComponent<InvulnerabilityComponent>(entityId))
                {
                    return;
                }

                healthComponent.CurrentHealth -= pendingDamageEventComponent.TotalDamage;

                if (ComponentManager.TryGetComponent<RegenerationComponent>(entityId, out var regenerationComponent))
                {
                    regenerationComponent.LastDamageTime = 0f;
                }

                Debug.Log($"{GetType().Name} {entityId}: dealt {pendingDamageEventComponent.TotalDamage} damage, health left: {healthComponent.CurrentHealth}");

                ComponentManager.RemoveComponent<PendingDamageEventComponent>(entityId);
            }
        }
    }
}
