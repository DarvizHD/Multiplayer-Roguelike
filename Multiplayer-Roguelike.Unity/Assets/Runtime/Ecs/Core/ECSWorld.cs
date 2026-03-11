using Runtime.ECS.Components;
using Runtime.ECS.Systems;

namespace Runtime.ECS.Core
{
    public class EcsWorld
    {
        public static EcsWorld DebugInstance { get; private set; }
        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }
        private ushort _nextEntityId;

        public EcsWorld()
        {
            DebugInstance = this;
            ComponentManager = new ComponentManager(64);
            SystemManager = new SystemManager(ComponentManager);
        }

        public ushort CreateEntity()
        {
            return _nextEntityId++;
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

        public void AddSystem<T>(T system) where T : BaseSystem
        {
            SystemManager.RegisterSystem(system);
        }
    }
}
