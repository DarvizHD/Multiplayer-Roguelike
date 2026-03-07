namespace Runtime.Ecs.Components
{
    public interface IComponentStorage<out T> where T : IComponent
    {
        ushort Count { get; }
        ushort[] EntityIds { get; }
        T[]  Components { get; }
        bool Has(ushort entityId);
        bool TryGet(ushort id, out IComponent component);
        T Get(ushort entityId);
        void Remove(ushort entityId);
    }
}
