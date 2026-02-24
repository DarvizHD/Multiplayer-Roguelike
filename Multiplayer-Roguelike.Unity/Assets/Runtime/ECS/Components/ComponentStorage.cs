using System.Collections.Generic;

namespace Runtime.ECS.Components
{
    public class ComponentStorage<T> : IComponentStorage<T> where T : IComponent
    {
        public IEnumerable<int> EntityIds => _components.Keys;

        private readonly Dictionary<int, T> _components = new Dictionary<int, T>();

        public void Add(int entityId, T component)
        {
            _components.Add(entityId, component);
        }

        public bool Has(int entityId)
        {
            return _components.ContainsKey(entityId);
        }

        public T Get(int entityId)
        {
            return _components[entityId];
        }

        public bool TryGet(int id, out IComponent component)
        {
            var success = _components.TryGetValue(id, out var componentId);  
            
            component = success ? componentId : default;
            
            return success;
        }
    }
}