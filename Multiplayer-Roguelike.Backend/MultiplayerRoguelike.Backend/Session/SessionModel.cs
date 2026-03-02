using Backend.Player.Collection;
using Shared.Models;

namespace Backend.Session
{
    public class SessionModel
    {
        public string Id { get; }

        public PlayerModelCollection Players { get; set; } = new();

        public WorldSharedModel WorldSharedModel { get; set; }

        public SessionModel(string id)
        {
            Id = id;
            WorldSharedModel = new WorldSharedModel(id);
        }
    }
}
