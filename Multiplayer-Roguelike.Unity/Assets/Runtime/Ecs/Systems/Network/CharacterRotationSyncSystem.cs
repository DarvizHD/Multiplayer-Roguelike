using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Network
{
    public class CharacterRotationSyncSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, RotationComponent, RotationSpeedComponent, NetworkControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterSharedModelComponent = _buffer.Components1[i];
                var rotationComponent = _buffer.Components2[i];
                var rotationSpeedComponent = _buffer.Components3[i];

                rotationComponent.Angle = Mathf.LerpAngle
                (
                    rotationComponent.Angle,
                    characterSharedModelComponent.CharacterSharedModel.Rotation.Value,
                    rotationSpeedComponent.Speed * deltaTime
                );
            }
        }
    }
}
