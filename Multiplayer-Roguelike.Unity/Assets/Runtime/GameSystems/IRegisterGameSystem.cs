namespace Runtime.GameSystems
{
    public interface IRegisterGameSystem<in T> : IGameSystem
    {
        void Register(T item);

        void Unregister(T item);
    }
}
