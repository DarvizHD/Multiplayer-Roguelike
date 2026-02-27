using ENet;

namespace Backend.Player
{
    public class PlayerModel
    {
        public string PlayerNickname { get; }
        public Peer Peer { get; }
        public string PartyId { get; set; } = string.Empty;

        public PlayerModel(string playerNickname, Peer peer)
        {
            PlayerNickname = playerNickname;
            Peer = peer;
        }
    }
}
