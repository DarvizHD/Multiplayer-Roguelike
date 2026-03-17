using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Movement
{
    public class CursorWorldPositionSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PlayerInputComponent, CursorWorldPositionComponent> _buffer = new();

        private readonly Camera _camera = Camera.main;
        private readonly Plane _groundPlane = new(Vector3.up, Vector3.zero);

        protected override void Update(int i, float deltaTime)
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

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }
    }
}
