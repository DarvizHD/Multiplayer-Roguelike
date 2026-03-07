using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Movement
{
    public class DrawTransformSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, RotationComponent, TransformComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var positionComponent = _buffer.Components1[i];
                var rotationComponent = _buffer.Components2[i];
                var transformComponent = _buffer.Components3[i];

                transformComponent.Transform.position = positionComponent.Position;
                transformComponent.Transform.rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);
            }
        }
    }
}
