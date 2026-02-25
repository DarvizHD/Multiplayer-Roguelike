using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;

namespace Runtime.ECS.Systems
{
    public class SeparationSystem : BaseSystem
    {
        private const float MinDistance = 2f;

        public SeparationSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(SeparationComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent = (PositionComponent)components[0];

            foreach (var (otherId, otherComponents) in ComponentManager.Query(typeof(PositionComponent)))
            {
                if (otherId <= id) continue;

                var otherPosition = (PositionComponent)otherComponents[0];

                var delta = positionComponent.Position - otherPosition.Position;
                delta.y = 0;

                var distance = delta.magnitude;

                if (distance is >= MinDistance or < 0.001f)
                {
                    continue;
                }

                var correction = delta.normalized * (MinDistance - distance);

                var currentIsPlayer = ComponentManager.HasComponent<PlayerComponent>(id);
                var otherIsPlayer = ComponentManager.HasComponent<PlayerComponent>(otherId);

                if (!currentIsPlayer)
                {
                    positionComponent.Position += correction * 0.5f;
                }

                if (!otherIsPlayer)
                {
                    otherPosition.Position -= correction * 0.5f;
                }
            }
        }
    }
}