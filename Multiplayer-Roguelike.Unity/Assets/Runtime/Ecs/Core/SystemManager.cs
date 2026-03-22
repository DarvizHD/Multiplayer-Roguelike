using System.Collections.Generic;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Core
{
    public class SystemManager
    {
        private readonly Dictionary<UpdateMode, List<BaseSystem>> _systems = new()
        {
            { UpdateMode.Update, new List<BaseSystem>() },
            { UpdateMode.FixedUpdate, new List<BaseSystem>() },
            { UpdateMode.LateUpdate, new List<BaseSystem>() },
            { UpdateMode.SendUpdate, new List<BaseSystem>() }
        };

        private readonly ComponentManager _componentManager;

        public SystemManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public void RegisterSystem<T>(UpdateMode updateMode = UpdateMode.Update) where T : BaseSystem, new()
        {
            var system = new T();

            system.Initialize(_componentManager);

            _systems[updateMode].Add(system);
        }

        public void RegisterSystem<T>(T system, UpdateMode updateMode = UpdateMode.Update) where T : BaseSystem
        {
            system.Initialize(_componentManager);

            _systems[updateMode].Add(system);
        }

        public void UpdateAll(float deltaTime)
        {
            foreach (var system in _systems[UpdateMode.Update])
            {
                system.UpdateAll(deltaTime);
            }
        }

        public void FixedUpdateAll(float fixedDeltaTime)
        {
            foreach (var system in _systems[UpdateMode.FixedUpdate])
            {
                system.UpdateAll(fixedDeltaTime);
            }
        }

        public void LateUpdateAll(float lateDeltaTime)
        {
            foreach (var system in _systems[UpdateMode.LateUpdate])
            {
                system.UpdateAll(lateDeltaTime);
            }
        }

        public void SendUpdate(float deltaTime)
        {
            foreach (var system in _systems[UpdateMode.SendUpdate])
            {
                system.UpdateAll(deltaTime);
            }
        }

        public void ClearSystems()
        {
            _systems.Clear();
        }
    }
}
