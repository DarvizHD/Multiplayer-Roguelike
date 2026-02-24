using System;

namespace Runtime.CustomAsync
{
    public class Scheduler
    {
        public static Scheduler Instance { get; } = new();

        public event Action<float> OnTick;
        public event Action OnAborted;

        private Scheduler()
        {
        }

        public void Update(float deltaTime)
        {
            OnTick?.Invoke(deltaTime);
        }

        public void AbortAll()
        {
            OnAborted?.Invoke();
        }
    }
}