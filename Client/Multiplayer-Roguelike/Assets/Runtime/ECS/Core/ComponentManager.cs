using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.ECS.Components;

namespace Runtime.ECS.Core
{
    public class ComponentManager
    {
        private readonly Dictionary<Type, IComponentStorage<IComponent>> _storage = new();

        private readonly Dictionary<Type, int> _pendingRemove = new();
        
        public void RegisterComponent<T>() where T : class, IComponent
        {
            _storage[typeof(T)] = new ComponentStorage<T>();
        }
        
        public void AddComponent<T>(int entityId, T component) where T : class, IComponent
        {
            GetStorage<T>().Add(entityId, component);
        }

        public void RemoveComponent<T>(int entityId) where T : class, IComponent
        {
            _pendingRemove.Add(typeof(T), entityId);
        }

        public object GetComponent(int entityId, Type componentType)
        {
            if (!_storage.TryGetValue(componentType, out var storage))
            {
                return null;
            }

            storage.TryGet(entityId, out var component);
            return _storage;
        }

        public T GetComponent<T>(int entityId) where T : class, IComponent
        {
            return (T) _storage[typeof(T)].Get(entityId);
        }

        public bool TryGetComponent<T>(int entityId, out T component) where T : class, IComponent
        {
            var success =  _storage[typeof(T)].TryGet(entityId, out var founded);

            component = (T) founded;
            
            return success;
        }

        public bool HasComponent<T>(int entityId) where T : class, IComponent
        {
            return _storage.ContainsKey(typeof(T));
        }

        public IEnumerable<int> GetAllEntities()
        {
            return _storage.Values.SelectMany(s => s.EntityIds).Distinct();
        }

        public IEnumerable<Type> GetComponentTypes(int entityId)
        {
            return from pair in _storage where pair.Value.Has(entityId) select pair.Key;
        }

        public IEnumerable<(int entityId, object[] components)> Query(params Type[] componentTypes)
        {
            var storages = componentTypes.Select(t => _storage[t]).ToArray();

            var entityIds = storages[0].EntityIds;

            foreach (var storage in storages.Skip(1))
            {
                entityIds = entityIds.Intersect(storage.EntityIds);
            }

            foreach (var id in entityIds)
            {
                var components = storages.Select(s => s.TryGet(id, out var component) ? component : null).ToArray();

                yield return (id, components);
            }
            
            RemoveComponents();
        }

        private ComponentStorage<T> GetStorage<T>() where T : class, IComponent
        {
            return (ComponentStorage<T>)_storage[typeof(T)];
        }

        private void RemoveComponents()
        {
            foreach (var pair in _pendingRemove)
            {
                _storage[pair.Key].Remove(pair.Value);
            }
            
            _pendingRemove.Clear();
        }
    }
}