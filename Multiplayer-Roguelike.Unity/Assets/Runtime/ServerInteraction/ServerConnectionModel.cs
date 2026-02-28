using System;
using ENet;
using Runtime.CustomAsync;

namespace Runtime.ServerInteraction
{
    public class ServerConnectionModel : IServerConnectionModel
    {
        public event Action PlayerConnect;
        public event Action PlayerDisconnect;

        public event Action<Packet> PacketReceived;

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

        public void SetPacket(Packet packet)
        {
            PacketReceived?.Invoke(packet);
        }
    }
}
