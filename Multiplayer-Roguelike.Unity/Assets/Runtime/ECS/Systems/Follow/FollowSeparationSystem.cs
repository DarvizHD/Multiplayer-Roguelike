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
            var positionQuery = ComponentManager.TupleQuery<PositionComponent>();
            var entityIds = positionQuery.entityId;
            var components = positionQuery.components;
            var count = positionQuery.count;

            foreach (var (entityId, positionComponent, separationComponent)
                     in ComponentManager.Query<PositionComponent, SeparationComponent>())
            {
                for (int i = 0; i < count; i++)
                {
                    var otherId = entityIds[i];
                    var otherComponent = components[i];

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
