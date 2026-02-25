using ENet;

namespace Runtime.ServerInteraction
{
    public interface IServerConnectionModel
    {
        Peer PlayerPeer { get; }
        Host PlayerHost { get; }

        void ConnectPlayer();
        void DisconnectPlayer();
        void CompletePlayerConnect();
    }
}
