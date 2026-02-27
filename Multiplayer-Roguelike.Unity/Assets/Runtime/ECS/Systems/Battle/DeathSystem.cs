using Runtime.ECS.Components.Health;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class DeathSystem : BaseSystem
    {
        public DeathSystem()
        {
            RegisterRequiredComponent(typeof(HealthComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, healthComponent)
                     in ComponentManager.Query<HealthComponent>())
            {

                if (healthComponent.CurrentHealth <= 0 && !ComponentManager.HasComponent<DeathTagComponent>(entityId))
                {
                    ComponentManager.AddComponent(entityId, new DeathTagComponent());

                    Debug.Log($"Entity {entityId} died");
                }
            }
        }
    }
}
