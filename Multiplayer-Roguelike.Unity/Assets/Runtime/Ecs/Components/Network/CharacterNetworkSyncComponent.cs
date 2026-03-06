using Shared.Models;

namespace Runtime.Ecs.Components.Network
{
    public class CharacterNetworkSyncComponent : IComponent
    {
        public readonly CharacterSharedModel CharacterSharedModel;

        public CharacterNetworkSyncComponent(CharacterSharedModel characterSharedModel)
        {
            CharacterSharedModel = characterSharedModel;
        }
    }
}
