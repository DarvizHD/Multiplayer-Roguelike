using System;
using System.Threading;
using Backend.CommandExecutors;
using Backend.Lobby.Collection;
using Backend.Player.Collection;
using ENet;

namespace Backend
{
    public class Server
    {
        private WorldModel _world;
        
        public async void Start()
        {
            _world = new WorldModel();
            
            var playerCollectionPresenter = new PlayerModelCollectionPresenter(_world.Players);
            playerCollectionPresenter.Enable();
            
            var lobbyCollectionPresenter = new LobbyModelCollectionPresenter(_world.Lobbies, _world);
            lobbyCollectionPresenter.Enable();

            Library.Initialize();
            
            var playerThread = new Thread(() => Update());
            playerThread.Start();
            
            Console.WriteLine("Multiplayer-Roguelike.Backend started!");
        }

        private void Update()
        {
            var playerAddress = new Address()
            {
                Port = 8080
            };

            var playerHost = new Host();
            playerHost.Create(playerAddress, 5, 100);

            var commandExecutorFactory = new CommandExecutorFactory(_world);
            
            while (true)
            {
                var polled = false;
                while (!polled)
                {
                    if (playerHost.CheckEvents(out var netEvent) <= 0)
                    {
                        if (playerHost.Service(15, out netEvent) <= 0)
                        {
                            break;
                        }
                        
                        polled = true;
                    }

                    switch (netEvent.Type)
                    {
                        case EventType.None:
                        default:
                            break;
                        case EventType.Connect:
                            Console.WriteLine($"{netEvent.Peer.ID} connected");
                            break;
                        case EventType.Disconnect:
                            Console.WriteLine($"{netEvent.Peer.ID} disconnected");
                            break;
                        case EventType.Receive:
                            commandExecutorFactory.CreateCommandExecutor(ref netEvent).Execute();
                            netEvent.Packet.Dispose();
                            break;
                        case EventType.Timeout:
                            Console.WriteLine($"{netEvent.Peer.ID} timed out");
                            break;
                    }

                    playerHost.Flush();
                }
            }
        }

        private void SendPacket(Peer peer, byte channelId, ref Packet packet)
        {
            if (!peer.Send(channelId, ref packet))
            {
                Console.WriteLine($"Error sending to peer {peer.ID} packet {channelId}");
            }
        }
    }
}