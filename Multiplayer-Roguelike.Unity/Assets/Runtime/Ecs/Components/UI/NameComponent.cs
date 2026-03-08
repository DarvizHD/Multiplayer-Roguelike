using Runtime.Ecs.Components;

namespace Runtime.UI.HUD
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
