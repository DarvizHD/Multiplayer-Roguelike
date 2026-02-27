using Runtime.ECS.Components;
using Runtime.ECS.Components.Camera;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Tags;
using UnityEngine;

namespace Runtime.ECS.Systems.CameraFocus
{
    public class CameraFocusSystem : BaseSystem
    {
        public CameraFocusSystem()
        {
            RegisterRequiredComponent(typeof(CameraTargetComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, cameraTargetComponent)
                     in ComponentManager.Query<CameraTargetComponent>())
            {

                var players = ComponentManager.Query<PositionComponent, PlayerTagComponent>();

                var sum = Vector3.zero;
                var count = 0;

                foreach (var (_, positionComponent, playerTagComponent) in players)
                {
                    sum += positionComponent.Position;
                    count++;
                }

                if (count == 0)
                {
                    return;
                }

                cameraTargetComponent.TargetPosition = sum / count;
            }
        }
    }
}
