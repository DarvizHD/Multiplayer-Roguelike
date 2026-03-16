using System.Collections.Generic;

namespace Runtime.Pools
{
    public abstract class Pool<T> where T : IPoolItem
    {
        protected T Prefab { get; }

        private readonly List<T> _pool = new List<T>();

        private readonly List<T> _inProgress = new List<T>();

        public Pool(T prefab)
        {
            Prefab = prefab;
        }

        public T Get()
        {
            T item = GetItemFromPool();

            AddItemToInProgress(item);

            item.OnComplete += ReturnToPool;
            item.Enable();

            return item;
        }

        protected T GetItemFromPool()
        {
            T item;

            if (_pool.Count > 0)
            {
                item = _pool[0];
                _pool.RemoveAt(0);
            }
            else
            {
                item = CreateItem();
            }

            return item;
        }

        protected void AddItemToInProgress(T item)
        {
            _inProgress.Add(item);
        }

        protected abstract T CreateItem();

        protected void ReturnToPool(IPoolItem item)
        {
            var typedItem = (T) item;

            typedItem.Disable();

            _inProgress.Remove(typedItem);

            _pool.Add(typedItem);

            typedItem.OnComplete -= ReturnToPool;
        }
    }
}
