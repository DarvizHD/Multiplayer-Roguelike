using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Network
{
    public class PositionInterpolationSystem : BaseSystem
    {
        private QueryBuffer<PositionInterpolationComponent, PositionComponent,
            DirectionComponent, MoveSpeedComponent,
            NetworkControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var interpolationComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];
                var directionComponent = _buffer.Components3[i];
                var moveSpeedComponent = _buffer.Components4[i];

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
