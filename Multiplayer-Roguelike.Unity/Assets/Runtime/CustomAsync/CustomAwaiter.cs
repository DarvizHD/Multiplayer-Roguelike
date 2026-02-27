using System;
using System.Runtime.CompilerServices;

namespace Runtime.CustomAsync
{
    public class CustomAwaiter : INotifyCompletion
    {
        private Action _continuations;

        public bool IsCompleted { get; private set; }

        public void Complete()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                _continuations?.Invoke();
                _continuations = null;
            }
        }

        public void OnCompleted(Action continuation)
        {
            _continuations += continuation;
        }

        public void GetResult()
        {
        }

        public CustomAwaiter GetAwaiter()
        {
            return this;
        }

        public void Dispose()
        {
            _continuations = null;
        }
    }
}
