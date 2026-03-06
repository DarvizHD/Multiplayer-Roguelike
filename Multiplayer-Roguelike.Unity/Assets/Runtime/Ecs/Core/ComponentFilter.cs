using System;
using Runtime.Ecs.Components;

namespace Runtime.Ecs.Core
{
    public class ComponentFilter
    {
        private readonly IComponentStorage<IComponent>[] _storages;

        public ComponentFilter(IComponentStorage<IComponent>[] storages)
        {
            _storages = storages;
        }

        public void Query<T>(ref QueryBuffer<T> buffer) where T : IComponent
        {
            var storage = _storages[ComponentId<T>.Id];
            buffer.Components = storage.Components as T[];
            buffer.Count = storage.Count;
            buffer.EntityIds = storage.EntityIds;
        }

        public void Query<T1, T2>(ref QueryBuffer<T1, T2> buffer)
            where T1 : IComponent
            where T2 : IComponent
        {
            buffer.Count = 0;

            var storage1 = _storages[ComponentId<T1>.Id];
            var storage2 = _storages[ComponentId<T2>.Id];

            var smallestStorage = storage1.Count <= storage2.Count ? storage1 : storage2;

            for (var i = 0; i < smallestStorage.Count; i++)
            {
                var entityId = smallestStorage.EntityIds[i];

                if (storage1.Has(entityId) && storage2.Has(entityId))
                {
                    if (buffer.Count >= buffer.EntityIds.Length)
                    {
                        Resize(ref buffer);
                    }

                    buffer.EntityIds[buffer.Count] = entityId;
                    buffer.Components1[buffer.Count] = (T1)storage1.Get(entityId);
                    buffer.Components2[buffer.Count] = (T2)storage2.Get(entityId);

                    buffer.Count++;
                }
            }
        }

        public void Query<T1, T2, T3>(ref QueryBuffer<T1, T2, T3> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            buffer.Count = 0;

            var storage1 = _storages[ComponentId<T1>.Id];
            var storage2 = _storages[ComponentId<T2>.Id];
            var storage3 = _storages[ComponentId<T3>.Id];

            var smallestStorage = storage1;

            if (storage2.Count < smallestStorage.Count)
                smallestStorage = storage2;

            if (storage3.Count < smallestStorage.Count)
                smallestStorage = storage3;

            for (var i = 0; i < smallestStorage.Count; i++)
            {
                var entityId = smallestStorage.EntityIds[i];

                if (storage1.Has(entityId) &&
                    storage2.Has(entityId) &&
                    storage3.Has(entityId))
                {
                    if (buffer.Count >= buffer.EntityIds.Length)
                    {
                        Resize(ref buffer);
                    }

                    buffer.EntityIds[buffer.Count] = entityId;
                    buffer.Components1[buffer.Count] = (T1)storage1.Get(entityId);
                    buffer.Components2[buffer.Count] = (T2)storage2.Get(entityId);
                    buffer.Components3[buffer.Count] = (T3)storage3.Get(entityId);

                    buffer.Count++;
                }
            }
        }

        public void Query<T1, T2, T3, T4>(ref QueryBuffer<T1, T2, T3, T4> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            buffer.Count = 0;

            var storage1 = _storages[ComponentId<T1>.Id];
            var storage2 = _storages[ComponentId<T2>.Id];
            var storage3 = _storages[ComponentId<T3>.Id];
            var storage4 = _storages[ComponentId<T4>.Id];

            var smallestStorage = storage1;

            if (storage2.Count < smallestStorage.Count)
                smallestStorage = storage2;

            if (storage3.Count < smallestStorage.Count)
                smallestStorage = storage3;

            if (storage4.Count < smallestStorage.Count)
                smallestStorage = storage4;

            for (var i = 0; i < smallestStorage.Count; i++)
            {
                var entityId = smallestStorage.EntityIds[i];

                if (storage1.Has(entityId) &&
                    storage2.Has(entityId) &&
                    storage3.Has(entityId) &&
                    storage4.Has(entityId))
                {
                    if (buffer.Count >= buffer.EntityIds.Length)
                    {
                        Resize(ref buffer);
                    }

                    buffer.EntityIds[buffer.Count] = entityId;
                    buffer.Components1[buffer.Count] = (T1)storage1.Get(entityId);
                    buffer.Components2[buffer.Count] = (T2)storage2.Get(entityId);
                    buffer.Components3[buffer.Count] = (T3)storage3.Get(entityId);
                    buffer.Components4[buffer.Count] = (T4)storage4.Get(entityId);

                    buffer.Count++;
                }
            }
        }

        public void Query<T1, T2, T3, T4, T5>(ref QueryBuffer<T1, T2, T3, T4, T5> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            buffer.Count = 0;

            var storage1 = _storages[ComponentId<T1>.Id];
            var storage2 = _storages[ComponentId<T2>.Id];
            var storage3 = _storages[ComponentId<T3>.Id];
            var storage4 = _storages[ComponentId<T4>.Id];
            var storage5 = _storages[ComponentId<T5>.Id];

            var smallestStorage = storage1;

            if (storage2.Count < smallestStorage.Count)
                smallestStorage = storage2;

            if (storage3.Count < smallestStorage.Count)
                smallestStorage = storage3;

            if (storage4.Count < smallestStorage.Count)
                smallestStorage = storage4;

            if (storage5.Count < smallestStorage.Count)
                smallestStorage = storage5;

            for (var i = 0; i < smallestStorage.Count; i++)
            {
                var entityId = smallestStorage.EntityIds[i];

                if (storage1.Has(entityId) &&
                    storage2.Has(entityId) &&
                    storage3.Has(entityId) &&
                    storage4.Has(entityId) &&
                    storage5.Has(entityId))
                {
                    if (buffer.Count >= buffer.EntityIds.Length)
                    {
                        Resize(ref buffer);
                    }

                    buffer.EntityIds[buffer.Count] = entityId;

                    buffer.Components1[buffer.Count] = (T1)storage1.Get(entityId);
                    buffer.Components2[buffer.Count] = (T2)storage2.Get(entityId);
                    buffer.Components3[buffer.Count] = (T3)storage3.Get(entityId);
                    buffer.Components4[buffer.Count] = (T4)storage4.Get(entityId);
                    buffer.Components5[buffer.Count] = (T5)storage5.Get(entityId);

                    buffer.Count++;
                }
            }
        }


        private void Resize<T1, T2>(ref QueryBuffer<T1, T2> buffer)
            where T1 : IComponent
            where T2 : IComponent
        {
            var newSize = buffer.EntityIds.Length * 2;

            Array.Resize(ref buffer.EntityIds, newSize);
            Array.Resize(ref buffer.Components1, newSize);
            Array.Resize(ref buffer.Components2, newSize);
        }

        private void Resize<T1, T2, T3>(ref QueryBuffer<T1, T2, T3> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            var newSize = buffer.EntityIds.Length * 2;

            Array.Resize(ref buffer.EntityIds, newSize);
            Array.Resize(ref buffer.Components1, newSize);
            Array.Resize(ref buffer.Components2, newSize);
            Array.Resize(ref buffer.Components3, newSize);
        }

        private void Resize<T1, T2, T3, T4>(ref QueryBuffer<T1, T2, T3, T4> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            var newSize = buffer.EntityIds.Length * 2;

            Array.Resize(ref buffer.EntityIds, newSize);
            Array.Resize(ref buffer.Components1, newSize);
            Array.Resize(ref buffer.Components2, newSize);
            Array.Resize(ref buffer.Components3, newSize);
            Array.Resize(ref buffer.Components4, newSize);
        }

        private void Resize<T1, T2, T3, T4, T5>(ref QueryBuffer<T1, T2, T3, T4, T5> buffer)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
            where T5 : IComponent
        {
            var newSize = buffer.EntityIds.Length * 2;

            Array.Resize(ref buffer.EntityIds, newSize);
            Array.Resize(ref buffer.Components1, newSize);
            Array.Resize(ref buffer.Components2, newSize);
            Array.Resize(ref buffer.Components3, newSize);
            Array.Resize(ref buffer.Components4, newSize);
            Array.Resize(ref buffer.Components5, newSize);
        }
    }
}
