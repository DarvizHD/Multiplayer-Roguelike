using Runtime.Ecs.Components.Camera;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.CameraFocus
{
    public class CameraFocusSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, PlayerTagComponent> _playersBuffer = new();
        private QueryBuffer<CameraTargetComponent> _cameraTargetBuffer = new();

        public override void Update(float deltaTime)
        {
            var players = ComponentManager.Query<PositionComponent, PlayerTagComponent, PlayerInputComponent>();

            ComponentManager.Filter.Query(ref _cameraTargetBuffer);

            for (var i = 0; i < _cameraTargetBuffer.Count; i++)
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

                var cameraTargetComponent = _cameraTargetBuffer.Components[i];

                cameraTargetComponent.TargetPosition = sum / count;
            }
        }
    }
}
