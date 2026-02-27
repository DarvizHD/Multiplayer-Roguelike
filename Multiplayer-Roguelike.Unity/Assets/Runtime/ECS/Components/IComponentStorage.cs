using System.Collections.Generic;

namespace Runtime.ECS.Components
{
    public interface IComponentStorage<out T> where T : IComponent
    {
        public int Count { get; }
        IEnumerable<int> EntityIds { get; }
        bool Has(int entityId);
        bool TryGet(int id, out IComponent component);

        T Get(int entityId);
        void Remove(int entityId);
    }
}
