using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Network
{
    public class CharacterRotationSendSystem : BaseSystem
    {
        private QueryBuffer<CharacterConnectionComponent, CharacterNetworkSyncComponent, RotationComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var characterConnectionComponent = _buffer.Components1[i];
                var characterNetworkSyncComponent = _buffer.Components2[i];
                var rotationComponent = _buffer.Components3[i];

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
