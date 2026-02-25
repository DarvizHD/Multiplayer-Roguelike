using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems
{
    public abstract class BaseSystem
    {
        private IEnumerable<Type> Components => _requiredComponents;

        private readonly List<Type> _requiredComponents = new();

        protected ComponentManager ComponentManager { get; private set; }

        public void Initialize(ComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public void Update(float deltaTime)
        {
            foreach (var (id, components) in ComponentManager.Query(Components.ToArray()))
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
