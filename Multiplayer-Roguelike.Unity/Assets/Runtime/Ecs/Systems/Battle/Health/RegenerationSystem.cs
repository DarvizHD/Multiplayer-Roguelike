using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle.Health
{
    public class RegenerationSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<HealthComponent, RegenerationComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
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
