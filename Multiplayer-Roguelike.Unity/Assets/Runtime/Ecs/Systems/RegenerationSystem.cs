using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems
{
    public class RegenerationSystem : BaseSystem
    {
        private QueryBuffer<HealthComponent, RegenerationComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);


            for (var i = 0; i < _buffer.Count; i++)
            {
                var healthComponent = _buffer.Components1[i];
                var regenerationComponent = _buffer.Components2[i];
                var entityId = _buffer.EntityIds[i];

                if (ComponentManager.HasComponent<DeathTagComponent>(entityId))
                {
                    return;
                }

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
