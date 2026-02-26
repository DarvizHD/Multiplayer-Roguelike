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

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var cameraTarget = components[0] as CameraTargetComponent;

            var players = ComponentManager.Query(typeof(PositionComponent), typeof(PlayerTagComponent));

            var sum = Vector3.zero;
            var count = 0;

            foreach (var (_, playerComponents) in players)
            {
                sum += ((PositionComponent)playerComponents[0]).Position;
                count++;
            }

            if (count == 0)
            {
                return;
            }

            cameraTarget.TargetPosition = sum / count;
        }
    }
}
