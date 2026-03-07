using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle.Health
{
    public class DeathAnimationSystem : BaseSystem
    {
        private QueryBuffer<DeathAnimationComponent> _deathAnimationBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _deathAnimationBuffer);

            for (var i = 0; i < _deathAnimationBuffer.Count; ++i)
            {
                var deathAnimationComponent = _deathAnimationBuffer.Components[i];
                var entityId = _deathAnimationBuffer.EntityIds[i];

                deathAnimationComponent.Timer -= deltaTime;

                if (deathAnimationComponent.Timer <= 0)
                {
                    if (ComponentManager.TryGetComponent<GameObjectComponent>(entityId, out var gameObjectComponent))
                    {
                        Object.Destroy(gameObjectComponent.GameObject);
                    }
                }
            }
        }
    }
}
