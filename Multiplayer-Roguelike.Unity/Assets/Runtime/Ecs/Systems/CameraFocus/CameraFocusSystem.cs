using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Components.Tags;
using UnityEngine;

namespace Runtime.Ecs.Systems.CameraFocus
{
    public class CameraFocusSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            var players = ComponentManager.Query<PositionComponent, PlayerTagComponent, PlayerInputComponent>();

            foreach (var (entityId, cameraTargetComponent) in ComponentManager.Query<CameraTargetComponent>())
            {
                var sum = Vector3.zero;
                var count = 0;

                foreach (var (_, positionComponent, playerTagComponent, playerInputComponent) in players)
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
