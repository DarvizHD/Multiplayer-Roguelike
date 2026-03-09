using Runtime.Ecs.Components;

namespace Runtime.ECS.Components
{
    public class CharacterNetworkEventComponent : IComponent
    {
        public string EventId;

        public CharacterNetworkEventComponent(string eventId)
        {
            EventId = eventId;
        }
    }
}
