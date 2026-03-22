using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.CameraFocus
{
    public class DrawCameraTransformSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CameraTargetComponent, TransformComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var cameraTargetComponent = _buffer.Components1[i];
            var transformComponent = _buffer.Components2[i];

            transformComponent.Transform.position = Vector3.Lerp(transformComponent.Transform.position, cameraTargetComponent.TargetPosition, deltaTime * 5f);
        }
    }
}
