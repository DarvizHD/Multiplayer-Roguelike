using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.ECS.Components;
using UnityEngine;

namespace Runtime.ECS.Core
{
    public class ComponentManager
    {
        private readonly Dictionary<Type, IComponentStorage<IComponent>> _storage = new();
        private readonly Dictionary<int, Dictionary<Type, int>> _pendingRemove = new();

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
            if (!_pendingRemove.ContainsKey(entityId))
            {
                _pendingRemove[entityId] = new Dictionary<Type, int>();
            }
            _pendingRemove[entityId][typeof(T)] = entityId;
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

        public T GetComponent<T>(int entityId) where T : class, IComponent
        {
            return (T)_storage[typeof(T)].Get(entityId);
        }

        public bool TryGetComponent<T>(int entityId, out T component) where T : class, IComponent
        {
            var success = _storage[typeof(T)].TryGet(entityId, out var founded);

            component = (T)founded;

            return success;
        }

        public bool HasComponent<T>(int entityId) where T : class, IComponent
        {
            return _storage[typeof(T)].Has(entityId);
        }

        public IEnumerable<int> GetAllEntities()
        {
            return _storage.Values.SelectMany(s => s.EntityIds).Distinct();
        }

        public IEnumerable<Type> GetComponentTypes(int entityId)
        {
            return from pair in _storage where pair.Value.Has(entityId) select pair.Key;
        }

        /*
        public IEnumerable<(int entityId, IComponent[] components)> Query(params Type[] componentTypes)
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
        */

        public IEnumerable<(int entityId, T1)> Query<T1>() where T1 : class, IComponent
        {
            foreach (var entityId in GetStorage<T1>().EntityIds)
            {
                yield return (entityId, GetStorage<T1>().Get(entityId));
            }
        }

        public IEnumerable<(int entityId, T1, T2)> Query<T1, T2>()
            where T1 : class, IComponent
            where T2 : class, IComponent
        {
            foreach (var entityId in GetStorage<T1>().EntityIds)
            {
                if (GetStorage<T2>().Has(entityId))
                {
                    yield return (entityId, GetStorage<T1>().Get(entityId), GetStorage<T2>().Get(entityId));
                }
            }
        }

        public IEnumerable<(int entityId, T1, T2, T3)> Query<T1, T2, T3>()
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
        {
            foreach (var entityId in GetStorage<T1>().EntityIds)
            {
                if (GetStorage<T2>().Has(entityId) && GetStorage<T3>().Has(entityId))
                {
                    yield return (entityId, GetStorage<T1>().Get(entityId), GetStorage<T2>().Get(entityId), GetStorage<T3>().Get(entityId));
                }
            }
        }

        public IEnumerable<(int entityId, T1, T2, T3, T4)> Query<T1, T2, T3, T4>()
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
            where T4 : class, IComponent
        {
            foreach (var entityId in GetStorage<T1>().EntityIds)
            {
                if (GetStorage<T2>().Has(entityId) && GetStorage<T3>().Has(entityId) && GetStorage<T4>().Has(entityId))
                {
                    yield return (entityId,
                        GetStorage<T1>().Get(entityId),
                        GetStorage<T2>().Get(entityId),
                        GetStorage<T3>().Get(entityId),
                        GetStorage<T4>().Get(entityId));
                }
            }
        }

        public IEnumerable<(int entityId, T1, T2, T3, T4, T5)> Query<T1, T2, T3, T4, T5>()
            where T1 : class, IComponent
            where T2 : class, IComponent
            where T3 : class, IComponent
            where T4 : class, IComponent
            where T5 : class, IComponent
        {
            foreach (var entityId in GetStorage<T1>().EntityIds)
            {
                if (GetStorage<T2>().Has(entityId) && GetStorage<T3>().Has(entityId) && GetStorage<T4>().Has(entityId) &&  GetStorage<T5>().Has(entityId))
                {
                    yield return (entityId,
                        GetStorage<T1>().Get(entityId),
                        GetStorage<T2>().Get(entityId),
                        GetStorage<T3>().Get(entityId),
                        GetStorage<T4>().Get(entityId),
                        GetStorage<T5>().Get(entityId));
                }
            }
        }

        private ComponentStorage<T> GetStorage<T>() where T : class, IComponent
        {
            return (ComponentStorage<T>)_storage[typeof(T)];
        }

        private void RemoveComponents()
        {
            foreach (var entityKv in _pendingRemove)
            {
                int entityId = entityKv.Key;
                foreach (var componentType in entityKv.Value.Keys)
                {
                    if (_storage.TryGetValue(componentType, out var storage))
                    {
                        storage.Remove(entityId);
                    }
                }
            }

            _pendingRemove.Clear();
        }

        public void RemoveEntity(int entityId)
        {
            foreach (var kv in _storage)
            {
                if (kv.Value.Has(entityId))
                {
                    if (!_pendingRemove.ContainsKey(entityId))
                    {
                        _pendingRemove[entityId] = new Dictionary<Type, int>();
                    }
                    _pendingRemove[entityId][kv.Key] = entityId;
                }
            }
            Debug.Log($"Entity {entityId} marked for removal");
        }
    }
}
