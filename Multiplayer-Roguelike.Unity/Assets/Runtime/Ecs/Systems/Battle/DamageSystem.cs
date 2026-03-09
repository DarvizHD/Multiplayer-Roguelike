using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.Battle
{
    public class DamageSystem : BaseSystem
    {
        private QueryBuffer<PendingDamageEventComponent, HealthComponent, AliveTagComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var healthComponent = _buffer.Components2[i];
                var pendingDamageEventComponent = _buffer.Components1[i];

                if (healthComponent.CurrentHealth <= 0 || ComponentManager.HasComponent<InvulnerabilityComponent>(entityId))
                {
                    continue;
                }

                healthComponent.CurrentHealth -= pendingDamageEventComponent.TotalDamage;

                if (healthComponent.CurrentHealth <= 0)
                {
                    ComponentManager.AddComponent(entityId, new DeathEventComponent());
                    ComponentManager.AddComponent(entityId, new DeathTagComponent());
                    ComponentManager.RemoveComponent<AliveTagComponent>(entityId);
                }

                if (ComponentManager.TryGetComponent<RegenerationComponent>(entityId, out var regenerationComponent))
                {
                    regenerationComponent.LastDamageTime = 0f;
                }

                ComponentManager.RemoveComponent<PendingDamageEventComponent>(entityId);
            }
        }
    }
}
