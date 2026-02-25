using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components.Player
{
    public class PlayerLookRotationSystem : BaseSystem
    {
        public PlayerLookRotationSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent));
            RegisterRequiredComponent(typeof(PlayerLookRotationComponent));
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var playerInputComponent = components[0] as PlayerInputComponent;
            var playerLookRotationComponent = components[1] as PlayerLookRotationComponent;
            var positionComponent = components[2] as PositionComponent;
            var rotationComponent = components[3] as RotationComponent;
            
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

                    var smoothness = playerLookRotationComponent.RotationSpeed * deltaTime;
                    
                    rotationComponent.Angle = Mathf.LerpAngle(targetAngle, rotationComponent.Angle, smoothness);                    
                }
            }
        }
    }
}