using ENet;
using Shared.Models;

namespace Backend.Player
{
    public class PlayerModel
    {
        public PlayerSharedModel PlayerSharedModel { get; }
        public Peer Peer { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsConnectingToSession { get; set; }

        public PlayerModel(string playerNickname, Peer peer)
        {
            PlayerSharedModel = new PlayerSharedModel(playerNickname);
            PlayerSharedModel.Nickname.Value = playerNickname;

            Peer = peer;
        }
    }
}
