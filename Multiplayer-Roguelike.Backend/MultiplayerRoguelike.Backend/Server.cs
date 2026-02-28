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
        private readonly ushort _port;

        private Host _host;
        private Thread _thread;
        private WorldModel _world;
        private CommandExecutorFactory _commandExecutorFactory;
        private bool _isRunning;

        public Server(ushort port)
        {
            _port = port;
        }

        public void Start()
        {
            _world = new WorldModel();

            var playerCollectionPresenter = new PlayerModelCollectionPresenter(_world.Players);
            playerCollectionPresenter.Enable();

            var lobbyCollectionPresenter = new LobbyModelCollectionPresenter(_world.Lobbies, _world);
            lobbyCollectionPresenter.Enable();

            Library.Initialize();

            var address = new Address { Port = _port };

            _host = new Host();
            _host.Create(address, 5, 2);

            _isRunning = true;

            _commandExecutorFactory = new CommandExecutorFactory(_world);

            _thread = new Thread(NetworkLoop);
            _thread.Start();

            Console.WriteLine($"Server started on port {_port}");
        }

        public void Stop()
        {
            _isRunning = false;

            _thread.Join();

            _host.Dispose();
            Library.Deinitialize();

            Console.WriteLine("Server stopped");
        }

        private void NetworkLoop()
        {
            while (_isRunning)
            {
                while (_host.CheckEvents(out var netEvent) > 0)
                {
                    HandleEvent(netEvent);
                }

                while (_host.Service(15, out var netEvent) > 0)
                {
                    HandleEvent(netEvent);
                }

                HandleTick();
            }
        }

        private void HandleEvent(Event netEvent)
        {
            switch (netEvent.Type)
            {
                case EventType.Connect:
                    Console.WriteLine($"{netEvent.Peer.ID} connected");
                    break;

                case EventType.Receive:
                    _commandExecutorFactory.CreateCommandExecutor(ref netEvent).Execute();
                    netEvent.Packet.Dispose();
                    break;

                case EventType.Disconnect:
                    Console.WriteLine($"{netEvent.Peer.ID} disconnected");
                    break;

                case EventType.Timeout:
                    Console.WriteLine($"{netEvent.Peer.ID} timed out");
                    break;
            }
        }

        public void HandleTick()
        {
            foreach (var player in _world.Players.Models.Values)
            {
                if (player.PlayerSharedModel.IsDirty)
                {
                    Console.WriteLine($"\nPlayer {player.PlayerSharedModel.Nickname} has changes");
                    player.PlayerSharedModel.GetChanges(out var changes);
                    foreach (var change in changes)
                    {
                        Console.WriteLine($"{change.Key}: {change.Value}");
                    }
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
