using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;

namespace Runtime.ECS.Systems
{
    public class SeparationSystem : BaseSystem
    {
        private const float MinDistance = 1.5f;

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
                if (otherId <= id)
                {
                    continue;
                }

                var otherPosition = (PositionComponent)otherComponents[0];

                var delta = positionComponent.Position - otherPosition.Position;
                delta.y = 0;

                var distance = delta.magnitude;

                if (distance is >= MinDistance or < 0.001f)
                {
                    continue;
                }

                var correction = delta.normalized * (MinDistance - distance);

                positionComponent.Position += correction;
                otherPosition.Position -= correction;
            }
        }
    }
}
