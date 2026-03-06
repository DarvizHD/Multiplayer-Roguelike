using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Rotation
{
    public class PlayerLookRotationSystem : BaseSystem
    {
        private QueryBuffer<PlayerInputComponent, PositionComponent, RotationComponent, RotationSpeedComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var playerInputComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];
                var rotationComponent = _buffer.Components3[i];
                var rotationSpeedComponent = _buffer.Components4[i];

                var mouseScreenPosition = playerInputComponent.PlayerControls.Gameplay.Look.ReadValue<Vector2>();
                var mouseWorldPosition = Camera.main.ScreenPointToRay(mouseScreenPosition);

                if (Physics.Raycast(mouseWorldPosition, out var hit))
                {
                    var lookPoint = hit.point;

                    var direction = lookPoint - positionComponent.Position;
                    direction.y = 0f;

                    if (direction.sqrMagnitude > 0.01f)
                    {
                        var targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
                        rotationComponent.Angle = Mathf.LerpAngle(rotationComponent.Angle, targetAngle, rotationSpeedComponent.Speed * deltaTime);
                    }
                }
            }
        }
    }
}
