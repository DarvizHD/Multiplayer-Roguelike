using System;
using System.Collections.Generic;

namespace Runtime.ECS.Components
{
    public class ComponentStorage<T> : IComponentStorage<T> where T : IComponent
    {
        public int Count => _count;
        public int[] EntityIds => _entityIds[.._count];

        public T[] Components => _components;

        private int[] _entityIds;

        private readonly Dictionary<int, int> _entityToComponentsMap = new();
        private T[] _components;
        private int _count;

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

            if (_count >= _components.Length)
            {
                Resize();
            }

            _entityIds[_count] = entityId;
            _components[_count] = component;
            _entityToComponentsMap[entityId] = _count;

            _count++;
        }

        public T Get(int entityId)
        {
            return (T)_components[_entityToComponentsMap[entityId]];
        }

        public bool TryGet(int entityId, out IComponent component)
        {
            if (_entityToComponentsMap.TryGetValue(entityId, out int index))
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

            var lastIndex = _count - 1;
            var lastEntity = _entityIds[lastIndex];

            _components[index] = _components[lastIndex];
            _entityIds[index] = lastEntity;

            _entityToComponentsMap[lastEntity] = index;

            _components[lastIndex] = default(T);
            _entityIds[lastIndex] = default;

            _entityToComponentsMap.Remove(entityId);

            _count--;
        }

        private void Resize()
        {
            int newSize = EntityIds.Length * 2;

            Array.Resize(ref _entityIds, newSize);
            Array.Resize(ref _components, newSize);
        }
    }
}
