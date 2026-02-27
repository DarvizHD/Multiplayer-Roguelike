using System.Collections.Generic;
using Runtime.ECS.Components;

namespace Runtime.ECS.Core
{
    public class ComponentManager
    {
        private readonly IComponentStorage<IComponent>[] _storages;

        private readonly Dictionary<int, int> _toRemoveEntityComponents = new();

        public ComponentManager(int maxComponentsTypes)
        {
            _storages = new IComponentStorage<IComponent>[maxComponentsTypes];
        }

        public void RegisterComponent<T>() where T : class, IComponent
        {
            var id = ComponentId<T>.Id;
            _storages[id] = new ComponentStorage<T>();
        }

        public void AddComponent<T>(int entityId, T component) where T : class, IComponent
        {
            GetStorage<T>().Add(entityId, component);
        }

        public void RemoveComponent<T>(int entityId) where T : class, IComponent
        {
            _toRemoveEntityComponents[ComponentId<T>.Id] = entityId;
        }

        public T GetComponent<T>(int entityId) where T : class, IComponent
        {
            return (T)_storages[ComponentId<T>.Id].Get(entityId);
        }

        public bool TryGetComponent<T>(int entityId, out T component) where T : class, IComponent
        {
            var success = _storages[ComponentId<T>.Id].TryGet(entityId, out var founded);

            component = (T)founded;

            return success;
        }

        public bool HasComponent<T>(int entityId) where T : class, IComponent
        {
            return _storages[ComponentId<T>.Id].Has(entityId);
        }

        public IEnumerable<int> GetAllEntities()
        {
            // TODO: Rework this
            yield break;

           // return _storages.SelectMany(s => s.EntityIds).Distinct();
        }

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

        private ComponentStorage<T> GetStorage<T>() where T : class, IComponent
        {
            return (ComponentStorage<T>) _storages[ComponentId<T>.Id];
        }

        public void RemoveEntity(int entityId)
        {
            // TODO: Rework this

        }
    }
}
