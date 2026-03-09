using Runtime.ECS.Components;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Movement
{
    public class CursorWorldPositionSystem : BaseSystem
    {
        private Camera _camera;
        private readonly Plane _groundPlane = new(Vector3.up, Vector3.zero);

        private QueryBuffer<PlayerInputComponent, CursorWorldPositionComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            _camera = Camera.main;

            for (var i = 0; i < _buffer.Count; i++)
            {
                var input = _buffer.Components1[i];
                var cursorPosition = _buffer.Components2[i];

                var screenPos = input.PlayerControls.Gameplay.Look.ReadValue<Vector2>();
                var ray = _camera.ScreenPointToRay(screenPos);

                if (_groundPlane.Raycast(ray, out var distance))
                {
                    cursorPosition.Position = ray.GetPoint(distance);
                }
            }
        }
    }
}
