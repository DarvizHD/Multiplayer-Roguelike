using System;
using System.Collections.Generic;

namespace Runtime.Ecs.Components
{
    public class ComponentStorage<T> : IComponentStorage<T> where T : IComponent
    {
        public ushort Count { get; private set; }

        public ushort[] EntityIds => _entityIds[..Count];

        public T[] Components => _components;

        private ushort[] _entityIds;

        private readonly Dictionary<ushort, ushort> _entityToComponentsMap = new();
        private T[] _components;

        public ComponentStorage(ushort initialCapacity = 16)
        {
            _entityIds = new ushort[initialCapacity];
            _components = new T[initialCapacity];
            _entityToComponentsMap = new Dictionary<ushort, ushort>();
        }

        public void Add(ushort entityId, T component)
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

        public T Get(ushort entityId)
        {
            return _components[_entityToComponentsMap[entityId]];
        }

        public bool TryGet(ushort entityId, out IComponent component)
        {
            if (_entityToComponentsMap.TryGetValue(entityId, out var index))
            {
                component = _components[index];
                return true;
            }

            component = default(T);
            return false;
        }

        public bool Has(ushort entityId)
        {
            return _entityToComponentsMap.ContainsKey(entityId);
        }

        public void Remove(ushort entityId)
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
