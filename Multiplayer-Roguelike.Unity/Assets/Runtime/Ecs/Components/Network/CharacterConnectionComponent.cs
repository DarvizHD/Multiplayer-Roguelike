using Runtime.ServerInteraction;

namespace Runtime.Ecs.Components.Network
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
