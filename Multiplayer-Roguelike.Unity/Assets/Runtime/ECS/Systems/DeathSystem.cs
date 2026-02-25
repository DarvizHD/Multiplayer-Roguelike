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

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var health = components[0] as HealthComponent;
            
            if (health.CurrentHealth <= 0 && !ComponentManager.HasComponent<DeathComponent>(id))
            {
                ComponentManager.AddComponent(id, new DeathComponent());
                
                Debug.Log($"Entity {id} died");
            }
        }
    }
}