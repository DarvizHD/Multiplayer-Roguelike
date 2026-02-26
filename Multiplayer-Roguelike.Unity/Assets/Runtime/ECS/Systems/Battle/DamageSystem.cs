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

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var pendingDamage = components[0] as PendingDamageEventComponent;
            var healthComponent = components[1] as HealthComponent;

            if (healthComponent.CurrentHealth <= 0 || ComponentManager.HasComponent<InvulnerabilityComponent>(id))
            {
                return;
            }

            healthComponent.CurrentHealth -= pendingDamage.TotalDamage;

            if (ComponentManager.TryGetComponent<RegenerationComponent>(id, out var regenerationComponent))
            {
                regenerationComponent.LastDamageTime = 0f;
            }

            Debug.Log($"{GetType().Name} {id}: dealt {pendingDamage.TotalDamage} damage, health left: {healthComponent.CurrentHealth}");

            ComponentManager.RemoveComponent<PendingDamageEventComponent>(id);
        }
    }
}
