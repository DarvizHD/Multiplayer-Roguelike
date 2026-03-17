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

        public virtual void Update(float deltaTime)
        {

        }

        protected abstract void Update(int i, float deltaTime);

        protected abstract void Query();

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
