using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using UnityEngine;

namespace Runtime.Ecs.Systems.Network
{
    public class PositionInterpolationSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, interpolationComponent, positionComponent, directionComponent, moveSpeedComponent, _)
                     in ComponentManager.Query<PositionInterpolationComponent, PositionComponent, DirectionComponent, MoveSpeedComponent, NetworkControllableTag>())
            {
                interpolationComponent.TotalTime += deltaTime;
                if (interpolationComponent.TargetTime - interpolationComponent.LastTime == 0)
                {
                    return;
                }

                var t = (interpolationComponent.TotalTime - interpolationComponent.TargetTime) / (interpolationComponent.TargetTime - interpolationComponent.LastTime);
                if (t <= 1)
                {
                    positionComponent.Position = Vector3.Lerp(interpolationComponent.LastPosition,
                        interpolationComponent.TargetPosition, t);
                }
                else
                {
                    positionComponent.Position += directionComponent.Direction.normalized * (moveSpeedComponent.Speed * deltaTime);
                }
            }
        }
    }
}
