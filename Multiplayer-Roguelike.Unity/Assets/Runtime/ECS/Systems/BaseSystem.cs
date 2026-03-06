using System;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems
{
    public abstract class BaseSystem
    {
        protected ComponentManager ComponentManager { get; private set; }

        public void Initialize(ComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public abstract void Update(float deltaTime);

        // TODO: Remove it in 58 usages.
        protected void RegisterRequiredComponent(Type componentType)
        {
        }
    }
}
