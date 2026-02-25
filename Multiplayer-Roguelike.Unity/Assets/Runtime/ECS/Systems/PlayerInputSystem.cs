using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerInputSystem : BaseSystem
    {
        public PlayerInputSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent)); 
            RegisterRequiredComponent(typeof(DirectionComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var playerInputComponent = components[0] as PlayerInputComponent;
            var directionComponent = components[1] as DirectionComponent;

            
            var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();
            directionComponent!.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        }
    }
}