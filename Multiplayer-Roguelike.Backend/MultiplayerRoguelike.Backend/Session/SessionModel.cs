using Backend.Player.Collection;
using Shared.Models;

namespace Backend.Session
{
    public class SessionModel
    {
        public string Id { get; }

        public PlayerModelCollection Players { get; set; } = new();

        public GameSessionSharedModel GameSessionSharedModel { get; set; }

        public SessionModel(string id)
        {
            Id = id;
            GameSessionSharedModel = new GameSessionSharedModel(id);
        }
    }
}
