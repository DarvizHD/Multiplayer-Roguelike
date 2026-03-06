namespace Runtime.Ecs.Components
{
    public interface IComponentStorage<out T> where T : IComponent
    {
        int Count { get; }
        int[] EntityIds { get; }
        T[]  Components { get; }
        bool Has(int entityId);
        bool TryGet(int id, out IComponent component);
        T Get(int entityId);
        void Remove(int entityId);
    }
}
