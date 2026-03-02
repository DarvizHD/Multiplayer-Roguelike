using ENet;
using Shared.Models;

namespace Backend.Player
{
    public class PlayerModel
    {
        public PlayerSharedModel PlayerSharedModel { get; }
        public Peer Peer { get; }
        public string SessionId { get; set; } = string.Empty;

        public PlayerModel(string playerNickname, Peer peer)
        {
            PlayerSharedModel = new PlayerSharedModel(playerNickname);
            PlayerSharedModel.Nickname.Value = playerNickname;

            Peer = peer;
        }
    }
}
