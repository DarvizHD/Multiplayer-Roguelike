using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Core
{
    public abstract class BaseSystem
    {
        protected ComponentManager ComponentManager { get; private set; }

        protected abstract IQueryBuffer Buffer { get; }

        public void Initialize(ComponentManager componentManager)
        {
            ComponentManager = componentManager;
        }

        protected abstract void Query();

        protected abstract void Update(int i, float deltaTime);

        public void UpdateAll(float deltaTime)
        {
            Query();

            if (Buffer == null)
            {
                Debug.Log($"{this.GetType().Name} UpdateAll");
            }

            for (var i = 0; i < Buffer.Count; i++)
            {
                Update(i, deltaTime);
            }
        }
    }
}
