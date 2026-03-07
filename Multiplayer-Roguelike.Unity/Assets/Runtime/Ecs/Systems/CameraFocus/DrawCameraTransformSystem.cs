using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.CameraFocus
{
    public class DrawCameraTransformSystem : BaseSystem
    {
        private QueryBuffer<CameraTargetComponent, TransformComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var cameraTargetComponent = _buffer.Components1[i];
                var transformComponent = _buffer.Components2[i];

                transformComponent.Transform.position = Vector3.Lerp(transformComponent.Transform.position, cameraTargetComponent.TargetPosition, deltaTime * 5f);
            }
        }
    }
}
