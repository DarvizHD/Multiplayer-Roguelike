using Runtime.ECS.Components;

namespace Runtime.ECS.Core
{
    public static class ComponentId<T> where T : IComponent
    {
        public static readonly int Id;

        static ComponentId()
        {
            Id = ComponentIdGenerator.NextId++;
        }
    }
}
