using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.ECS.Components;

namespace Runtime.ECS.Core
{
    public class ComponentManager
    {
        private readonly Dictionary<Type, IComponentStorage<IComponent>> _storage = new Dictionary<Type, IComponentStorage<IComponent>>();

        public void AddComponent<T>(int entityId, T component) where T : class, IComponent
        {
            GetStorage<T>().Add(entityId, component);
        }
        
        public IEnumerable<int> GetAllEntities()
        {
            return _storage.Values.SelectMany(s => s.EntityIds).Distinct();
        }
        
        public IEnumerable<Type> GetComponentTypes(int entityId)
        {
            return from kvp in _storage where kvp.Value.Has(entityId) select kvp.Key;
        }
        
        public object GetComponent(int entityId, Type componentType)
        {
            if (!_storage.TryGetValue(componentType, out var storage))
            {
                return null;
            }
            
            storage.TryGet(entityId, out var component);
            return component;
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
                var components = storages
                    .Select(s => s.TryGet(id, out var component) ? component : null)
                    .ToArray();
                
                yield return (id, components);
            }
        }
        private ComponentStorage<T> GetStorage<T>() where T : class, IComponent
        {
            if (!_storage.TryGetValue(typeof(T), out var storage))
            {
                _storage[typeof(T)] = new ComponentStorage<T>();
            }
            
            return (ComponentStorage<T>) _storage[typeof(T)];
        }
    }
}