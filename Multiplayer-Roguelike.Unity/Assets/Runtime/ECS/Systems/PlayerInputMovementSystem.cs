using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerInputMovementSystem : BaseSystem
    {
        public PlayerInputMovementSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, playerInputComponent, directionComponent)
                     in ComponentManager.Query<PlayerInputComponent, DirectionComponent>())
            {
                var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();

                directionComponent!.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            }
        }
    }
}
