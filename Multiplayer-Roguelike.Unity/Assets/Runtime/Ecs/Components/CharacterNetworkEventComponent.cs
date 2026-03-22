namespace Runtime.Ecs.Components
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
