using Shared.Models.Common;
using Shared.Models.Lobby;
using Shared.Properties;

namespace Shared.Models.Player
{
    public class PlayerSharedModel : SharedModel
    {
        public readonly Property<string> Nickname = new Property<string>("nickname", string.Empty);
        public readonly LobbySharedModel Lobby = new LobbySharedModel("lobby");

        public PlayerSharedModel(string id) : base(id)
        {
            Children.Add(Nickname.Id, Nickname);
            Children.Add(Lobby.Id, Lobby);
        }
    }
}
