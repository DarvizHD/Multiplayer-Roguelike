using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class DamageSystem : BaseSystem
    {
        public QueryBuffer<PendingDamageEventComponent, HealthComponent, AliveTagComponent> _buffer = new();

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
                    ComponentManager.RemoveComponent<AliveTagComponent>(entityId);
                    ComponentManager.AddComponent(entityId, new DeathEventComponent());
                    ComponentManager.AddComponent(entityId, new DeathTagComponent());
                }

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
