using Runtime.Components;

namespace Runtime.Core
{
    public class ECS
    {
        private readonly ComponentManager _componentManager;
        
        private readonly SystemManager _systemManager;

        public ECS()
        {
            _componentManager = new ComponentManager();
            
            _systemManager = new SystemManager(_componentManager);
        }
        
        public void Update(float deltaTime)
        {
            _systemManager.UpdateAll(deltaTime);
        }

        public void AddEntityComponent<T>(int entityId, T component) where T : class, IComponent
        {
            _componentManager.AddComponent<T>(entityId, component);
        }

        public void AddSystem<T>() where T : Systems.BaseSystem, new()
        {
            _systemManager.RegisterSystem<T>();
        }
    }
}