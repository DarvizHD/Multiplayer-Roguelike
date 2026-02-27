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

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, healthComponent, regenerationComponent)
                     in ComponentManager.Query<HealthComponent, RegenerationComponent>())
            {
                if (ComponentManager.HasComponent<DeathTagComponent>(entityId))
                    return;

                regenerationComponent.LastDamageTime += deltaTime;

                if (regenerationComponent.LastDamageTime >= regenerationComponent.Cooldown &&
                    healthComponent.CurrentHealth < healthComponent.MaxHealth)
                {
                    healthComponent.CurrentHealth = Mathf.Min(
                        healthComponent.CurrentHealth + regenerationComponent.RegenerationRate * deltaTime,
                        healthComponent.MaxHealth
                    );
                }
            }
        }
    }
}
