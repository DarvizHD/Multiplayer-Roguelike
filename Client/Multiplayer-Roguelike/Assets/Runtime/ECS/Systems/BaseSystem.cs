using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Core;

namespace Runtime.Systems
{
    public abstract class BaseSystem
    {
        private IEnumerable<Type> Components => _requiredComponents;
        
        private readonly List<Type> _requiredComponents = new List<Type>();
        
        private ComponentManager _componentManager;
        
        public void Initialize(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public void Update(float deltaTime)
        {
            foreach (var (id, components) in _componentManager.Query(Components.ToArray()))
            {
                Update(id, components, deltaTime);
            }
        }

        protected abstract void Update(int id, object[] components, float deltaTime);

        protected void RegisterRequiredComponent(Type componentType)
        {
            _requiredComponents.Add(componentType);
        }
    }
}