using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Shared.Commands;

namespace Runtime.Ecs.Systems.Network
{
    public class CharacterRotationSendSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, characterConnectionComponent, characterNetworkSyncComponent, rotationComponent, _) in
                     ComponentManager.Query<CharacterConnectionComponent, CharacterNetworkSyncComponent, RotationComponent, LocalControllableTag>())
            {
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
