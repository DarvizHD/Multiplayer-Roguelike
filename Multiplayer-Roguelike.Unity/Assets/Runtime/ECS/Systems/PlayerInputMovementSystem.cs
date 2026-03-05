using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using Shared.Commands;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerInputMovementSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, playerInputComponent, directionComponent)
                     in ComponentManager.Query<PlayerInputComponent, DirectionComponent>())
            {
                var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();
                directionComponent.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            }
        }
    }
}
