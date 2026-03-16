using System;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class CharacterRotationSendSystem : BaseSystem
    {
        private QueryBuffer<CharacterConnectionComponent, CharacterNetworkSyncComponent, RotationComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterConnectionComponent = _buffer.Components1[i];
                var characterNetworkSyncComponent = _buffer.Components2[i];
                var rotationComponent = _buffer.Components3[i];

                if (Math.Abs(rotationComponent.Angle - characterNetworkSyncComponent.CharacterSharedModel.Rotation.Value) < 1f)
                {
                    continue;
                }

                var rotateCommand = new RotateCommand
                (
                    characterNetworkSyncComponent.CharacterSharedModel.Id,
                    rotationComponent.Angle
                );

                rotateCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
