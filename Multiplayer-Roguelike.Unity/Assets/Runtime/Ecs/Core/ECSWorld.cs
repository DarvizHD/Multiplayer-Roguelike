using Runtime.Ecs.Components;
using Runtime.Ecs.Systems;

namespace Runtime.Ecs.Core
{
    public class EcsWorld
    {
        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }

        public EcsWorld()
        {
            ComponentManager = new ComponentManager(64);
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
