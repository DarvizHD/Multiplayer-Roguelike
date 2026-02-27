using System;

namespace Backend.ModelCollections
{
    public interface IModelCollection<out T>
    {
        event Action<T> OnAdded;
        event Action<T> OnRemoved;
    }
}
