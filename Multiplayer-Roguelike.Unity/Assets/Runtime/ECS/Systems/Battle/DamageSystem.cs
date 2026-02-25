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
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var pendingDamage = components[0] as PendingDamageEventComponent;
            if (ComponentManager.HasComponent<DeathComponent>(id))
            {
                Debug.Log($"Entity {id} is dead, damage ignored");
                ComponentManager.RemoveComponent<PendingDamageEventComponent>(id);
                return;
            }

            if (ComponentManager.HasComponent<InvulnerabilityComponent>(id))
            {
                Debug.Log($"Entity {id} is invulnerable, damage blocked");
                ComponentManager.RemoveComponent<PendingDamageEventComponent>(id);
                return;
            }

            if (ComponentManager.TryGetComponent<HealthComponent>(id, out var health))
            {
                health.CurrentHealth -= pendingDamage.TotalDamage;

                if (health.CurrentHealth < 0) health.CurrentHealth = 0;

                if (ComponentManager.TryGetComponent<RegenerationComponent>(id, out var regen))
                {
                    regen.LastDamageTime = 0f;
                }

                Debug.Log(
                    $"{GetType().Name} {id}: dealt {pendingDamage.TotalDamage} damage, health left: {health.CurrentHealth}");
            }

            ComponentManager.RemoveComponent<PendingDamageEventComponent>(id);
        }
    }
}
