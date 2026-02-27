using Runtime.ECS.Components;
using Runtime.ECS.Systems;

namespace Runtime.ECS.Core
{
    public class ECSWorld
    {
        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }

        public ECSWorld()
        {
            ComponentManager = new ComponentManager();
            SystemManager = new SystemManager(ComponentManager);
        }

        public void Update(float deltaTime)
        {
            SystemManager.UpdateAll(deltaTime);
            ComponentManager.RemoveComponents();
        }

        public void RegisterComponent<T>() where T : class, IComponent
        {
            ComponentManager.RegisterComponent<T>();
        }

        public void AddEntityComponent<T>(int entityId, T component) where T : class, IComponent
        {
            ComponentManager.AddComponent(entityId, component);
        }

        public void AddSystem<T>() where T : BaseSystem, new()
        {
            SystemManager.RegisterSystem<T>();
        }
    }
}
