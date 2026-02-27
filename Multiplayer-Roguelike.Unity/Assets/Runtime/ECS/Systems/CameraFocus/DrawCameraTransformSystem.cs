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

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, cameraTargetComponent, transformComponent)
                     in ComponentManager.Query<CameraTargetComponent, TransformComponent>())
            {

                transformComponent.Transform.position = Vector3.Lerp(transformComponent.Transform.position, cameraTargetComponent.TargetPosition, deltaTime * 5f);

            }
        }
    }
}
