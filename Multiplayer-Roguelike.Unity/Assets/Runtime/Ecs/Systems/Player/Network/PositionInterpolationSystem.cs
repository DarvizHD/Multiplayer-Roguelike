using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class PositionInterpolationSystem : BaseSystem
    {
        private const float _softThreshold = 0.25f;
        private const float _hardThreshold = 4f;

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

                var delta = (positionComponent.Position - interpolationComponent.TargetPosition).sqrMagnitude;

                if (delta is > _softThreshold and < _hardThreshold)
                {
                    positionComponent.Position = Vector3.Lerp(positionComponent.Position, interpolationComponent.TargetPosition,  0.1f);
                }
                else if (delta >= _hardThreshold)
                {
                    positionComponent.Position = interpolationComponent.TargetPosition;
                }
                else
                {
                    positionComponent.Position += directionComponent.Direction.normalized * (moveSpeedComponent.Speed * deltaTime);
                }
            }
        }
    }
}
