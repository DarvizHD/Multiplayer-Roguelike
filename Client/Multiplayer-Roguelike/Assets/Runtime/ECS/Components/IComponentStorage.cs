using System.Collections.Generic;

namespace Runtime.Components
{
    public interface IComponentStorage<out T> where T : IComponent
    {
        public IEnumerable<int> EntityIds { get; }
        bool TryGet(int id, out IComponent component);
    }
}