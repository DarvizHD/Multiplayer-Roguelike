using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.CameraFocus
{
    public class CameraFocusSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, PlayerTagComponent> _playersBuffer = new();
        private QueryBuffer<CameraTargetComponent> _cameraTargetBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _playersBuffer);
            ComponentManager.Filter.Query(ref _cameraTargetBuffer);

            for (var i = 0; i < _cameraTargetBuffer.Count; i++)
            {
                var sum = Vector3.zero;
                var count = 0;

                for (var k = 0; k < _playersBuffer.Count; k++)
                {
                    var positionComponent = _playersBuffer.Components1[k];
                    sum += positionComponent.Position;
                    count++;
                }

                if (count == 0)
                {
                    return;
                }

                var cameraTargetComponent = _cameraTargetBuffer.Components[i];

                cameraTargetComponent.TargetPosition = sum / count;
            }
        }
    }
}
