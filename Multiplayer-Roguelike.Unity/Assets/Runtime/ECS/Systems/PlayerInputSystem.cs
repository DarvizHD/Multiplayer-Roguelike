using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerInputSystem : BaseSystem
    {
        private readonly PlayerControls _controls;

        public PlayerInputSystem()
        {
            _controls = new PlayerControls();
            _controls.Enable();

            RegisterRequiredComponent(typeof(PlayerComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var directionComponent = components[1] as DirectionComponent;

            var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
            directionComponent!.Direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        }
    }
}
