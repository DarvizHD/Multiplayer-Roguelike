using Runtime.ECS.Components.Health;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class RegenerationSystem : BaseSystem
    {
        public RegenerationSystem()
        {
            RegisterRequiredComponent(typeof(HealthComponent));
            RegisterRequiredComponent(typeof(RegenerationComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            if (ComponentManager.HasComponent<DeathComponent>(id))
                return;

            var health = components[0] as HealthComponent;
            var regeneration = components[1] as RegenerationComponent;

            regeneration.LastDamageTime += deltaTime;

            if (regeneration.LastDamageTime >= regeneration.Cooldown &&
                health.CurrentHealth < health.MaxHealth)
            {
                health.CurrentHealth = Mathf.Min(
                    health.CurrentHealth + regeneration.RegenerationRate * deltaTime,
                    health.MaxHealth
                );
            }
        }
    }
}
