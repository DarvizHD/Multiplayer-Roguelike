using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Health;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class DamageSystem : BaseSystem
    {
        public DamageSystem()
        {
            RegisterRequiredComponent(typeof(PendingDamageEventComponent));
            RegisterRequiredComponent(typeof(HealthComponent));
        }

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
