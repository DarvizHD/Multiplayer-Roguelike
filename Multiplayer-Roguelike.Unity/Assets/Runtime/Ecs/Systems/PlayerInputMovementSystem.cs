using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems
{
    public class PlayerInputMovementSystem : BaseSystem
    {
        private QueryBuffer<PlayerInputComponent, DirectionComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var playerInputComponent = _buffer.Components1[i];
                var directionComponent = _buffer.Components2[i];

                var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();
                directionComponent.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            }
        }
    }
}
