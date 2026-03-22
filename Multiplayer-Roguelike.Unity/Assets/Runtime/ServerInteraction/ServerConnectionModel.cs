using System;
using ENet;
using Runtime.CustomAsync;

namespace Runtime.ServerInteraction
{
    public class ServerConnectionModel : IServerConnectionModel
    {
        private IServerConnectionModel _serverConnectionModelImplementation;
        public event Action<string, string> PlayerConnect;
        public event Action PlayerDisconnect;

        public event Action<Packet> PlayerPacketReceived;
        public event Action<Packet> WorldPacketReceived;

        public CustomAwaiter CompletePlayerConnectAwaiter { get; private set; } = new();

        public Peer PlayerPeer { get; set; }
        public Host PlayerHost { get; set; }

        public void ConnectPlayer(string address,  string nickname)
        {
            PlayerConnect?.Invoke(address, nickname);
        }

        public void DisconnectPlayer()
        {
            PlayerDisconnect?.Invoke();
        }

        public void CompletePlayerConnect()
        {
            CompletePlayerConnectAwaiter.Complete();
            CompletePlayerConnectAwaiter = new CustomAwaiter();
        }

        public void SetPlayerPacket(Packet packet)
        {
            PlayerPacketReceived?.Invoke(packet);
        }

        public void SetWorldPacket(Packet packet)
        {
            WorldPacketReceived?.Invoke(packet);
        }
    }
}
