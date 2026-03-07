using Runtime.Ecs.Components;

namespace Runtime.Ecs.Core
{
    public class QueryBuffer<T> where T : IComponent
    {
        public ushort Count;

        public ushort[] EntityIds;

        public T[] Components;

        public QueryBuffer(ushort initialCapacity = 32)
        {
            EntityIds = new ushort[initialCapacity];
            Components = new T[initialCapacity];
            Count = 0;
        }
    }

    public class QueryBuffer<T1, T2> where T1 : IComponent where T2 : IComponent
    {
        public ushort Count = 0;

        public ushort[] EntityIds;

        public T1[] Components1;

        public T2[] Components2;

        public QueryBuffer(ushort initialCapacity = 32)
        {
            EntityIds = new ushort[initialCapacity];
            Components1 = new T1[initialCapacity];
            Components2 = new T2[initialCapacity];
            Count = 0;
        }
    }

    public class QueryBuffer<T1, T2, T3>
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        public ushort Count = 0;

        public ushort[] EntityIds;

        public T1[] Components1;
        public T2[] Components2;
        public T3[] Components3;

        public QueryBuffer(ushort initialCapacity = 32)
        {
            EntityIds = new ushort[initialCapacity];
            Components1 = new T1[initialCapacity];
            Components2 = new T2[initialCapacity];
            Components3 = new T3[initialCapacity];
        }
    }

    public class QueryBuffer<T1, T2, T3, T4>
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        public ushort Count = 0;

        public ushort[] EntityIds;

        public T1[] Components1;
        public T2[] Components2;
        public T3[] Components3;
        public T4[] Components4;

        public QueryBuffer(ushort initialCapacity = 32)
        {
            EntityIds = new ushort[initialCapacity];
            Components1 = new T1[initialCapacity];
            Components2 = new T2[initialCapacity];
            Components3 = new T3[initialCapacity];
            Components4 = new T4[initialCapacity];
        }
    }

    public class QueryBuffer<T1, T2, T3, T4, T5>
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        public ushort Count = 0;

        public ushort[] EntityIds;

        public T1[] Components1;
        public T2[] Components2;
        public T3[] Components3;
        public T4[] Components4;
        public T5[] Components5;

        public QueryBuffer(ushort initialCapacity = 32)
        {
            EntityIds = new ushort[initialCapacity];
            Components1 = new T1[initialCapacity];
            Components2 = new T2[initialCapacity];
            Components3 = new T3[initialCapacity];
            Components4 = new T4[initialCapacity];
            Components5 = new T5[initialCapacity];
        }
    }
}
