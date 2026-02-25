using Backend.Lobby.Collection;
using Backend.Player.Collection;

namespace Backend
{
    public class WorldModel
    {
        public PlayerModelCollection Players { get; set; } = new();

        public LobbyModelCollection Lobbies { get; set; } = new();
    }
}