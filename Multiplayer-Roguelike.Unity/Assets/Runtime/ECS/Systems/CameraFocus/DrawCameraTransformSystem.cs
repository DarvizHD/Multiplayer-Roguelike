using Runtime.ECS.Components;
using Runtime.ECS.Components.Camera;
using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.CameraFocus
{
    public class DrawCameraTransformSystem : BaseSystem
    {
        public DrawCameraTransformSystem()
        {
            RegisterRequiredComponent(typeof(CameraTargetComponent));
            RegisterRequiredComponent(typeof(TransformComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var cameraTarget = (CameraTargetComponent)components[0];
            var transformComponent = (TransformComponent)components[1];

            transformComponent.Transform.position = Vector3.Lerp(transformComponent.Transform.position, cameraTarget.TargetPosition, deltaTime * 5f);
        }
    }
}
