using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class DeathSystem : BaseSystem
    {
        private QueryBuffer<HealthComponent> _healthComponentBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _healthComponentBuffer);

            for (var i = 0; i < _healthComponentBuffer.Count; i++)
            {
                var healthComponent = _healthComponentBuffer.Components[i];
                var entityId = _healthComponentBuffer.EntityIds[i];

                if (healthComponent.CurrentHealth <= 0 && !ComponentManager.HasComponent<DeathTagComponent>(entityId))
                {
                    ComponentManager.AddComponent(entityId, new DeathTagComponent());

                    Debug.Log($"Entity {entityId} died");
                }
            }
        }
    }
}
