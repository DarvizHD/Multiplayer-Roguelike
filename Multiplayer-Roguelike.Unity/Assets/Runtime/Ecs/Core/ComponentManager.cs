using System.Collections.Generic;
using Runtime.Ecs.Components;

namespace Runtime.Ecs.Core
{
    public class ComponentManager
    {
        public ComponentFilter Filter { get; private set; }

        private readonly IComponentStorage<IComponent>[] _storages;

        private readonly Dictionary<int, int> _toRemoveEntityComponents = new();

        public ComponentManager(int maxComponentsTypes)
        {
            _storages = new IComponentStorage<IComponent>[maxComponentsTypes];

            Filter = new ComponentFilter(_storages);
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
            return (ComponentStorage<T>)_storages[ComponentId<T>.Id];
        }

        public void RemoveEntity(int entityId)
        {
            // TODO: Rework this
        }
    }
}
