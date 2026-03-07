using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Tools;
using Shared.Commands.Player;

namespace Runtime.ECS.Systems.Network
{
    public class CharacterPositionSendSystem : BaseSystem
    {
        private QueryBuffer<CharacterConnectionComponent, CharacterNetworkSyncComponent,
            PositionComponent, DirectionComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterConnectionComponent = _buffer.Components1[i];
                var characterNetworkSyncComponent = _buffer.Components2[i];
                var positionComponent = _buffer.Components3[i];
                var directionComponent = _buffer.Components4[i];

                var moveCommand = new MoveCommand
                (
                    characterNetworkSyncComponent.CharacterSharedModel.Id,
                    positionComponent.Position.ToSharedVector3(),
                    directionComponent.Direction.ToSharedVector3()
                );

                moveCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
