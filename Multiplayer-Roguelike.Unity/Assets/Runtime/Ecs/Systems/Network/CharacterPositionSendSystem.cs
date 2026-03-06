using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Tools;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Network
{
    public class CharacterPositionSendSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, characterConnectionComponent, characterNetworkSyncComponent, positionComponent, directionComponent, _) in
                     ComponentManager.Query<CharacterConnectionComponent, CharacterNetworkSyncComponent, PositionComponent, DirectionComponent, LocalControllableTag>())
            {
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
