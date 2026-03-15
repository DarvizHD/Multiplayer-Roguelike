using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Backend.CommandExecutors.Common;
using Backend.CommandExecutors.Player;
using Backend.Lobby.Collection;
using Backend.Navigation;
using Backend.Player.Collection;
using Backend.Session.Collection;
using ENet;
using Shared.Commands.Player;
using Shared.Protocol;

namespace Backend
{
    public class Server
    {
        private const int _tickRate = 32;
        private const float _tickInterval = 1f / _tickRate;
        private readonly ushort _port;

        private Host _host;
        private Thread _thread;
        private WorldModel _world;
        private CommandExecutorFactory _commandExecutorFactory;
        private bool _isRunning;

        private int _hadleCount = 0;

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

            var sessionCollectionPresenter = new SessionModelCollectionPresenter(_world.Sessions, _world);
            sessionCollectionPresenter.Enable();

            var navigationPresenter = new NavigationPresenter(_world);
            navigationPresenter.Enable();

            Library.Initialize();

            var address = new Address { Port = _port };

            _host = new Host();
            _host.Create(address, 5, 2);

            _isRunning = true;

            _commandExecutorFactory = new CommandExecutorFactory(_world);

            _thread = new Thread(Update);
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

        private void Update()
        {
            while (true)
            {
                _world.ServerSystems.Update(_tickInterval);

                HandlePlayers();
                HandleSessions();

                var polled = false;
                while (!polled)
                {
                    if (_host.CheckEvents(out var netEvent) <= 0)
                    {
                        if (_host.Service(15, out netEvent) <= 0)
                        {
                            break;
                        }

                        polled = true;
                    }

                    HandleEvent(netEvent);

                    _host.Flush();
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

        private int recieveCount = 0;

        private void HandleEvent(Event netEvent)
        {
            switch (netEvent.Type)
            {
                case EventType.Connect:
                    Console.WriteLine($"{netEvent.Peer.ID} connected");
                    break;

                case EventType.Receive:
                    recieveCount++;
                    Console.WriteLine($"{netEvent.Peer.ID} received {recieveCount}");

                    _commandExecutorFactory.CreateCommandExecutor(ref netEvent).Execute();
                    netEvent.Packet.Dispose();
                    break;

                case EventType.Disconnect:
                    Console.WriteLine($"{netEvent.Peer.ID} disconnected");
                    break;

                case EventType.Timeout:
                    Console.WriteLine($"{netEvent.Peer.ID} timed out");

                    var player = _world.Players.Models.Values.FirstOrDefault(p => p.Peer.ID == netEvent.Peer.ID);
                    if (player != null)
                    {
                        var logoutCommand = new LogoutCommand(player.PlayerSharedModel.Nickname.Value);
                        var logoutExecutor = new LogoutCommandExecutor(logoutCommand, _world, player.Peer);
                        logoutExecutor.Execute();
                    }

                    break;
            }
        }

        private void HandlePlayers()
        {
            foreach (var player in _world.Players.Models.Values.Where(p => p.IsActive))
            {
                if (player.PlayerSharedModel.IsDirty || player.IsConnectingToSession)
                {
                    var protocol = new NetworkProtocol();
                    var packet = default(Packet);

                    if (player.IsConnectingToSession)
                    {
                        player.PlayerSharedModel.WriteAll(protocol);
                        player.IsConnectingToSession = player.SessionId != string.Empty;
                    }
                    else
                    {
                        player.PlayerSharedModel.Write(protocol);
                    }

                    packet.Create(protocol.Stream.GetBuffer());

                    SendPacket(player.Peer, 0, ref packet);
                }
            }
        }

        private void HandleSessions()
        {
            foreach (var session in _world.Sessions.Models.Values)
            {
                var worldSharedModel = session.GameSessionSharedModel;
                if (worldSharedModel.IsDirty || session.Players.Models.Values.Any(p => p.IsConnectingToSession))
                {
                    var protocol = new NetworkProtocol();
                    worldSharedModel.Write(protocol);

                    var fullWorldProtocol = new NetworkProtocol();
                    worldSharedModel.WriteAll(fullWorldProtocol);

                    foreach (var player in session.Players.Models.Values.Where(p => p.IsActive))
                    {
                        var packet = default(Packet);
                        packet.Create(player.IsConnectingToSession
                            ? fullWorldProtocol.Stream.GetBuffer()
                            : protocol.Stream.GetBuffer());
                        player.IsConnectingToSession = false;
                        SendPacket(player.Peer, 1, ref packet);
                    }
                }
            }
        }
    }
}
