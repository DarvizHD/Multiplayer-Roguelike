using ENet;

namespace Runtime.ServerInteraction
{
    public interface IServerConnectionModel
    {
        Peer PlayerPeer { get; }
        Host PlayerHost { get; }

        void ConnectPlayer(string nickname, string address);
        void DisconnectPlayer();
        void CompletePlayerConnect();
    }
}
