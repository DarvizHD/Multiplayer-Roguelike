using Runtime.ServerInteraction;

namespace Runtime.ECS.Components.Network
{
    public class CharacterConnectionComponent : IComponent
    {
        public readonly ServerConnectionModel ServerConnectionModel;

        public CharacterConnectionComponent(ServerConnectionModel serverConnectionModel)
        {
            ServerConnectionModel = serverConnectionModel;
        }
    }
}
