using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using UnityEngine;

namespace Runtime.Ecs.Systems.CameraFocus
{
    public class DrawCameraTransformSystem : BaseSystem
    {
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
