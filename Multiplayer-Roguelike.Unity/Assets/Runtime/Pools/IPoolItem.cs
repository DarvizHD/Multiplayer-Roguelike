using System;

public interface IPoolItem
{
    event Action<IPoolItem> OnComplete;

    void Enable();

    void Disable();
}