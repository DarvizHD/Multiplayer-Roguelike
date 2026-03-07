using Backend.Lobby.Collection;
using Backend.Player.Collection;
using Backend.ServerSystems;
using Backend.Session.Collection;

namespace Backend
{
    public class WorldModel
    {
        public PlayerModelCollection Players { get; set; } = new();

        public LobbyModelCollection Lobbies { get; set; } = new();

        public SessionModelCollection Sessions { get; set; } = new();

        public ServerSystemCollection  ServerSystems { get; set; } = new();
    }
}
