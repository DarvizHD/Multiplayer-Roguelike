using System;
using System.Collections.Generic;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Core
{
    public class SystemManager
{
        private readonly Dictionary<UpdateMode, List<BaseSystem>> _systems = new()
        {
            {UpdateMode.Update, new List<BaseSystem>()},
            {UpdateMode.FixedUpdate, new List<BaseSystem>()},
            {UpdateMode.LateUpdate, new List<BaseSystem>()}
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

        public void FixedUpdateAll(float fixedDeltaTime)
        {
            foreach (var system in _systems[UpdateMode.FixedUpdate])
            {
                system.Update(fixedDeltaTime);
            }
        }

        public void LateUpdateAll(float lateDeltaTime)
        {
            foreach (var system in _systems[UpdateMode.LateUpdate])
            {
                system.Update(lateDeltaTime);
            }
        }

        public void UpdateAll(float deltaTime)
        {
            foreach (var system in _systems[UpdateMode.Update])
            {
                system.Update(deltaTime);
            }
        }
    }
}
