using System;
using System.Collections.Generic;
using Runtime.Systems;

namespace Runtime.Core
{
    public class SystemManager
    {
        private readonly Dictionary<Type, BaseSystem> _systems = new();
        
        private readonly ComponentManager _componentManager;

        public SystemManager(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public void RegisterSystem<T>() where T : Systems.BaseSystem, new()
        {
            var system = new T();
            
            system.Initialize(_componentManager);
            
            _systems[typeof(T)] = system;
        }

        public void UnregisterSystem<T>()
        {
            _systems.Remove(typeof(T));
        }
        
        public void UpdateAll(float deltaTime)
        {
            foreach (var system in _systems.Values)
            {
                system.Update(deltaTime);
            }
        }
    }
}