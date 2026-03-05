using Backend.CommandExecutors;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Network;

namespace Runtime.ECS.Systems.Network
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
