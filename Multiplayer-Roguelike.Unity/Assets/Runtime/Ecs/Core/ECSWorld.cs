using Runtime.Ecs.Components;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Core
{
    public class EcsWorld
    {
        public static EcsWorld DebugInstance { get; private set; }
        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }
        private ushort _nextEntityId;

        private const float _sentTickRate = 1 / 32f;
        private float _ticks = 0f;

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
            if (_ticks >= _sentTickRate)
            {
                SystemManager.SendUpdate(_sentTickRate);
                _ticks = 0f;
            }
            _ticks += deltaTime;

            SystemManager.UpdateAll(deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            SystemManager.FixedUpdateAll(fixedDeltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            SystemManager.LateUpdateAll(deltaTime);
            ComponentManager.RemoveComponents();
        }

        public void RegisterComponent<T>() where T : class, IComponent
        {
            ComponentManager.RegisterComponent<T>();
        }

        public void ClearComponents()
        {
            ComponentManager.ClearComponents();
        }

        public void AddEntityComponent<T>(ushort entityId, T component) where T : class, IComponent
        {
            ComponentManager.AddComponent(entityId, component);
        }

        public void AddSystem<T>(UpdateMode updateMode = UpdateMode.Update) where T : BaseSystem, new()
        {
            SystemManager.RegisterSystem<T>(updateMode);
        }

        public void AddSystem<T>(T system, UpdateMode updateMode = UpdateMode.Update) where T : BaseSystem
        {
            SystemManager.RegisterSystem(system);
        }

        public void ClearSystems()
        {
            SystemManager.ClearSystems();
        }
    }
}
