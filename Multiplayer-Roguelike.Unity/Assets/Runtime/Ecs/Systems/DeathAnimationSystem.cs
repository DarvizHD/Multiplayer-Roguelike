using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using UnityEngine;

namespace Runtime.Ecs.Systems
{
    public class DeathAnimationSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, deathAnimationComponent)
                     in ComponentManager.Query<DeathAnimationComponent>())
            {
                deathAnimationComponent.Timer -= deltaTime;

                if (deathAnimationComponent.Timer <= 0)
                {
                    if (ComponentManager.TryGetComponent<GameObjectComponent>(entityId, out var gameObjectComponent))
                    {
                        Object.Destroy(gameObjectComponent.GameObject);
                    }

                    ComponentManager.RemoveEntity(entityId);
                }
            }
        }
    }
}
