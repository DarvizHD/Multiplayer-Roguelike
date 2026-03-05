using Shared.Models;

namespace Runtime.ECS.Components.Network
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
