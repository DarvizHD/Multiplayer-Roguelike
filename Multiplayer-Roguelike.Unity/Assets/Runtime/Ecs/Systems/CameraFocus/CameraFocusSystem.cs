using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.CameraFocus
{
    public class CameraFocusSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _cameraTargetBuffer;
        private QueryBuffer<PositionComponent, PlayerTagComponent, CameraFollowTagComponent> _playersBuffer = new();
        private QueryBuffer<CameraTargetComponent> _cameraTargetBuffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _playersBuffer);
            ComponentManager.Filter.Query(ref _cameraTargetBuffer);
        }

        protected override void Update(int i, float deltaTime)
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
