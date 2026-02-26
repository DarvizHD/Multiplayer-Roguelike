using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using UnityEngine;

namespace Runtime.ECS.Systems.Rotation
{
    public class PlayerLookRotationSystem : BaseSystem
    {
        public PlayerLookRotationSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent));
            RegisterRequiredComponent(typeof(PlayerLookRotationTagComponent));
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
            RegisterRequiredComponent(typeof(RotationSpeedComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var playerInputComponent = components[0] as PlayerInputComponent;
            var playerLookRotationComponent = components[1] as PlayerLookRotationTagComponent;
            var positionComponent = components[2] as PositionComponent;
            var rotationComponent = components[3] as RotationComponent;
            var rotationSpeedComponent = components[4] as RotationSpeedComponent;

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

                    var smoothness = rotationSpeedComponent.Speed * deltaTime;

                    rotationComponent.Angle = Mathf.LerpAngle(targetAngle, rotationComponent.Angle, smoothness);
                }
            }
        }
    }
}
