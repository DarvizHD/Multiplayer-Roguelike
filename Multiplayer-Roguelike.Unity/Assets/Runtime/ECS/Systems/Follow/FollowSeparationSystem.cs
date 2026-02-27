using Runtime.ECS.Components.Movement;

namespace Runtime.ECS.Systems.Follow
{
    public class FollowSeparationSystem : BaseSystem
    {
        private const float MinDistance = 1.5f;

        public FollowSeparationSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(SeparationComponent));
        }


        public override void Update(float deltaTime)
        {
            var positionQuery = ComponentManager.Query<PositionComponent>();

            foreach (var (entityId, positionComponent, separationComponent)
                     in ComponentManager.Query<PositionComponent, SeparationComponent>())
            {
                foreach (var (otherId, otherComponent) in positionQuery)
                {
                    if (otherId <= entityId)
                    {
                        continue;
                    }

                    var otherPosition = otherComponent;

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
}
