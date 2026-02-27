using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.Follow
{
    public class FollowSystem : BaseSystem
    {
        private const float StopDistance = 1f;

        public FollowSystem()
        {
            RegisterRequiredComponent(typeof(FollowComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
            RegisterRequiredComponent(typeof(PositionComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, followComponent, directionComponent, positionComponent)
                     in ComponentManager.Query<FollowComponent, DirectionComponent, PositionComponent>())
            {
                var toTarget = followComponent.Target.position - positionComponent.Position;
                toTarget.y = 0f;

                var distance = toTarget.magnitude;

                if (distance <= StopDistance)
                {
                    directionComponent.Direction = Vector3.zero;
                    return;
                }

                directionComponent.Direction = toTarget.normalized;
            }
        }
    }
}
