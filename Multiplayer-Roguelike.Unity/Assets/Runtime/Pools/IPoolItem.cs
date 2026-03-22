using System;

namespace Runtime.Pools
{
    public interface IPoolItem
    {
        event Action<IPoolItem> OnComplete;

        void Enable();

        void Disable();
    }
}