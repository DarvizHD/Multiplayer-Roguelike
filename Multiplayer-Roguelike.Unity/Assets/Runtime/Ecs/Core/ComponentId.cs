using Runtime.Ecs.Components;

namespace Runtime.Ecs.Core
{
    public static class ComponentId<T> where T : IComponent
    {
        public static readonly ushort Id;

        static ComponentId()
        {
            Id = ComponentIdGenerator.NextId++;
        }
    }
}
