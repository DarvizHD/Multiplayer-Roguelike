namespace Runtime.Ecs.Components.UI
{
    public class NameComponent : IComponent
    {
        public string Name;

        public NameComponent(string name)
        {
            Name = name;
        }
    }
}
