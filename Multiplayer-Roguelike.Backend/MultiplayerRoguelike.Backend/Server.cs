using System;
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
            var currentTime = DateTime.Now;

            while (_isRunning)
            {
                var nextTime = DateTime.Now;
                var deltaTime = (nextTime - currentTime).TotalSeconds;
                currentTime = nextTime;
                _world.ServerSystems.Update((float)deltaTime);

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

        private void HandleEvent(Event netEvent)
        {
            switch (netEvent.Type)
            {
                case EventType.Connect:
                    Console.WriteLine($"{netEvent.Peer.ID} connected");
                    break;

                case EventType.Receive:
                    Console.WriteLine($"packet received channel: {netEvent.ChannelID}, size: {netEvent.Packet.Length}");
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

                    var buffer = protocol.Stream.ToArray();
                    packet.Create(buffer, buffer.Length, PacketFlags.Reliable);
                    Console.WriteLine($"Player packet bytes: {BitConverter.ToString(protocol.Stream.ToArray())}");


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

                    Console.WriteLine($"World packet bytes: {BitConverter.ToString(fullWorldProtocol.Stream.ToArray())}");

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
