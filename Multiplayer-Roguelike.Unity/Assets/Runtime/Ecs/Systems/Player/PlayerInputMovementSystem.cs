using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Player
{
    public class PlayerInputMovementSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PlayerInputComponent, DirectionComponent, AliveTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var playerInputComponent = _buffer.Components1[i];
            var directionComponent = _buffer.Components2[i];

            var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();
            directionComponent.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        }
    }
}
