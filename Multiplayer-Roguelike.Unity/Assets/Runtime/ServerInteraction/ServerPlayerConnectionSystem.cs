using System;
using Runtime.GameSystems;
using UnityEngine;
using EventType = ENet.EventType;

namespace Runtime.ServerInteraction
{
    public class ServerPlayerConnectionSystem : IGameSystem
    {
        public string Id => "ServerPlayerConnectionSystem";
        
        private readonly ServerConnectionModel _serverConnectionModel;
        
        public ServerPlayerConnectionSystem(ServerConnectionModel serverConnectionModel)
        {
            _serverConnectionModel = serverConnectionModel;
        }
        
        public void Update(float deltaTime)
        {
            var host = _serverConnectionModel.PlayerHost;

            if (host.CheckEvents(out var netEvent) <= 0 || host.Service(0, out netEvent) <= 0)
            {
                return;
            }

            switch (netEvent.Type)
            {
                case EventType.None:
                    break;
                case EventType.Connect:
                    _serverConnectionModel.CompletePlayerConnect();
                    Debug.Log("Server connected");
                    break;
                case EventType.Disconnect:
                    Debug.Log("Server disconnected");
                    break;
                case EventType.Receive:
                    netEvent.Packet.Dispose();
                    break;
                case EventType.Timeout:
                    Debug.Log("Server time out");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}