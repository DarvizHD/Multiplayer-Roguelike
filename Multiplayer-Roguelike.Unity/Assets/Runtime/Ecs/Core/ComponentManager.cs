using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Ecs.Components;

namespace Runtime.Ecs.Core
{
    public class ComponentManager
    {
        public ComponentFilter Filter { get; private set; }

        private readonly IComponentStorage<IComponent>[] _storages;

        private readonly Dictionary<ushort, ushort> _toRemoveEntityComponents = new();

        public ComponentManager(ushort maxComponentsTypes)
        {
            _storages = new IComponentStorage<IComponent>[maxComponentsTypes];

            Filter = new ComponentFilter(_storages);
        }

        public void RegisterComponent<T>() where T : class, IComponent
        {
            var id = ComponentId<T>.Id;
            _storages[id] = new ComponentStorage<T>();
        }

        public void AddComponent<T>(ushort entityId, T component) where T : class, IComponent
        {
            var storage = (ComponentStorage<T>)_storages[ComponentId<T>.Id];

            storage.Add(entityId, component);
        }

        public void RemoveComponent<T>(ushort entityId) where T : class, IComponent
        {
            _toRemoveEntityComponents[ComponentId<T>.Id] = entityId;
        }

        public T GetComponent<T>(ushort entityId) where T : class, IComponent
        {
            return (T)_storages[ComponentId<T>.Id].Get(entityId);
        }

        public bool TryGetComponent<T>(ushort entityId, out T component) where T : class, IComponent
        {
            var success = _storages[ComponentId<T>.Id].TryGet(entityId, out var founded);

            component = (T)founded;

            return success;
        }

        public bool HasComponent<T>(ushort entityId) where T : class, IComponent
        {
            return _storages[ComponentId<T>.Id].Has(entityId);
        }

        public void RemoveComponents()
        {
            if (_toRemoveEntityComponents.Count == 0)
            {
                return;
            }

            foreach (var toRemoveComponentPair in _toRemoveEntityComponents)
            {
                _storages[toRemoveComponentPair.Key].Remove(toRemoveComponentPair.Value);
            }

            _toRemoveEntityComponents.Clear();
        }

        public List<ushort> GetAllEntities()
        {
            var entities = new HashSet<ushort>();

            foreach (var storage in _storages)
            {
                if (storage == null)
                {
                    continue;
                }

                foreach (var entity in storage.EntityIds)
                {
                    entities.Add(entity);
                }
            }

            return new List<ushort>(entities);
        }

        public List<Type> GetComponentTypes(ushort entityId)
        {
            var types = new List<Type>();

            for (ushort i = 0; i < _storages.Length; i++)
            {
                var storage = _storages[i];

                if (storage == null)
                {
                    continue;
                }

                if (storage.Has(entityId))
                {
                    types.Add(storage.GetType());
                }
            }

            return types;
        }

        public List<IComponent> GetAllComponents(ushort entityId)
        {
            var result = new List<IComponent>();

            foreach (var storage in _storages)
            {
                if (storage == null)
                {
                    continue;
                }

                if (storage.TryGet(entityId, out var component))
                {
                    result.Add(component);
                }
            }

            return result;
        }
    }
}
