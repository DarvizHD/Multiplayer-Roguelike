using Runtime.ECS.Components;
using Runtime.ECS.Components.Health;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class DeathAnimationSystem : BaseSystem
    {
        public DeathAnimationSystem()
        {
            RegisterRequiredComponent(typeof(DeathAnimationComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var deathAnimation = components[0] as DeathAnimationComponent;

            deathAnimation.Timer -= deltaTime;

            if (deathAnimation.Timer <= 0)
            {
                if (ComponentManager.TryGetComponent<GameObjectComponent>(id, out var gameObjectComponent))
                {
                    Object.Destroy(gameObjectComponent.GameObject);
                }

                ComponentManager.RemoveEntity(id);
            }
        }
    }
}
