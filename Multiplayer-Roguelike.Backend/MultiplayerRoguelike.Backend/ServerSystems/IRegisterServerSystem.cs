namespace Backend.ServerSystems
{
    public interface IRegisterServerSystem<in T> : IServerSystem
    {
        void Register(T item);

        void Unregister(T item);
    }
}
