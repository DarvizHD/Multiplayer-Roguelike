using System;
using System.Collections.Generic;

namespace Runtime.Ecs.Components
{
    public class ComponentStorage<T> : IComponentStorage<T> where T : IComponent
    {
        public int Count { get; private set; }

        public int[] EntityIds => _entityIds[..Count];

        public T[] Components => _components;

        private int[] _entityIds;

        private readonly Dictionary<int, int> _entityToComponentsMap = new();
        private T[] _components;

        public ComponentStorage(int initialCapacity = 16)
        {
            _entityIds = new int[initialCapacity];
            _components = new T[initialCapacity];
            _entityToComponentsMap = new Dictionary<int, int>();
        }

        public void Add(int entityId, T component)
        {
            if (_entityToComponentsMap.TryGetValue(entityId, out var existingIndex))
            {
                _components[existingIndex] = component;
                return;
            }

            if (Count >= _components.Length)
            {
                Resize();
            }

            _entityIds[Count] = entityId;
            _components[Count] = component;
            _entityToComponentsMap[entityId] = Count;

            Count++;
        }

        public T Get(int entityId)
        {
            return _components[_entityToComponentsMap[entityId]];
        }

        public bool TryGet(int entityId, out IComponent component)
        {
            if (_entityToComponentsMap.TryGetValue(entityId, out var index))
            {
                component = _components[index];
                return true;
            }

            component = default(T);
            return false;
        }

        public bool Has(int entityId)
        {
            return _entityToComponentsMap.ContainsKey(entityId);
        }

        public void Remove(int entityId)
        {
            if (!_entityToComponentsMap.TryGetValue(entityId, out var index))
            {
                return;
            }

            var lastIndex = Count - 1;
            var lastEntity = _entityIds[lastIndex];

            _components[index] = _components[lastIndex];
            _entityIds[index] = lastEntity;

            _entityToComponentsMap[lastEntity] = index;

            _components[lastIndex] = default;
            _entityIds[lastIndex] = default;

            _entityToComponentsMap.Remove(entityId);

            Count--;
        }

        private void Resize()
        {
            var newSize = EntityIds.Length * 2;

            Array.Resize(ref _entityIds, newSize);
            Array.Resize(ref _components, newSize);
        }
    }
}
