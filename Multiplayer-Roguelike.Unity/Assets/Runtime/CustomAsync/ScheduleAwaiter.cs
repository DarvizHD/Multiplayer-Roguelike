using System;
using System.Runtime.CompilerServices;

namespace Runtime.CustomAsync
{
    public class ScheduleAwaiter : INotifyCompletion
    {
        private Action _continuations;
        public bool IsCompleted { get; private set; }

        private float _awaitTime;

        public ScheduleAwaiter(float awaitTime)
        {
            _awaitTime = awaitTime;
        }

        public void Start()
        {
            Scheduler.Instance.OnTick += OnTick;
            Scheduler.Instance.OnAborted += Stop;
        }

        public void Stop()
        {
            Scheduler.Instance.OnTick -= OnTick;
            Scheduler.Instance.OnAborted -= Stop;

            IsCompleted = true;
            _continuations?.Invoke();
            _continuations = null;
        }

        private void OnTick(float deltaTime)
        {
            _awaitTime -= deltaTime;
            if (_awaitTime <= 0)
            {
                Stop();
            }
        }

        public void OnCompleted(Action continuation)
        {
            _continuations += continuation;
        }

        public void GetResult()
        {
        }

        public ScheduleAwaiter GetAwaiter()
        {
            return this;
        }
    }
}