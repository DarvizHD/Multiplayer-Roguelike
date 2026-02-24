using Runtime.ECS.Components;
using Runtime.ECS.Systems;

namespace Runtime.ECS.Core
{
    public class ECS
    {
        public ComponentManager ComponentManager { get; }
        public  SystemManager SystemManager { get; }

        public ECS()
        {
            ComponentManager = new ComponentManager();
            SystemManager = new SystemManager(ComponentManager);
        }
        
        public void Update(float deltaTime)
        {
            SystemManager.UpdateAll(deltaTime);
        }

        public void AddEntityComponent<T>(int entityId, T component) where T : class, IComponent
        {
            ComponentManager.AddComponent<T>(entityId, component);
        }

        public void AddSystem<T>() where T : BaseSystem, new()
        {
            SystemManager.RegisterSystem<T>();
        }
    }
}