using Runtime.Ecs.Components;
using Runtime.Ecs.Systems;
using Runtime.ECS.Systems;

namespace Runtime.Ecs.Core
{
    public class EcsWorld
    {
        public static EcsWorld DebugInstance { get; private set; }

        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }

        public EcsWorld()
        {
            DebugInstance = this;
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

        public void AddEntityComponent<T>(ushort entityId, T component) where T : class, IComponent
        {
            ComponentManager.AddComponent(entityId, component);
        }

        public void AddSystem<T>() where T : BaseSystem, new()
        {
            SystemManager.RegisterSystem<T>();
        }
    }
}
