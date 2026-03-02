using System;
using ENet;
using Runtime.CustomAsync;

namespace Runtime.ServerInteraction
{
    public class ServerConnectionModel : IServerConnectionModel
    {
        public event Action PlayerConnect;
        public event Action PlayerDisconnect;

        public event Action<Packet> PlayerPacketReceived;
        public event Action<Packet> WorldPacketReceived;

        public CustomAwaiter CompletePlayerConnectAwaiter { get; private set; } = new();

        public Peer PlayerPeer { get; set; }
        public Host PlayerHost { get; set; }

        public void ConnectPlayer()
        {
            PlayerConnect?.Invoke();
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
