using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Core
{
    public abstract class BaseSystem
    {
        protected ComponentManager ComponentManager { get; private set; }

        public void Initialize(ComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        public abstract void Update(float deltaTime);
    }
}
