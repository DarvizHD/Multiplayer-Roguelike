namespace Backend.ServerSystems
{
    public interface IServerSystem
    {
        void Update(float deltaTime);
        string Id { get; }
    }
}
