using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class CharacterRotationSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CharacterNetworkSyncComponent, RotationComponent, RotationSpeedComponent, NetworkControllableTag> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
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
