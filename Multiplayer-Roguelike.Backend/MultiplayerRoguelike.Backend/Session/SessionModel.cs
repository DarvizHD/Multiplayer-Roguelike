using Backend.Enemies;
using Backend.Navigation;
using Backend.Player.Collection;
using Shared.Models;

namespace Backend.Session
{
    public class SessionModel
    {
        public string Id { get; }

        public PlayerModelCollection Players { get; } = new();

        public GameSessionSharedModel GameSessionSharedModel { get; }

        public EnemyModelCollection  Enemies { get; } = new();

        public NavMeshModel NavMesh { get; } = new();

        public SessionModel(string id)
        {
            Id = id;
            GameSessionSharedModel = new GameSessionSharedModel(id);
        }
    }
}
