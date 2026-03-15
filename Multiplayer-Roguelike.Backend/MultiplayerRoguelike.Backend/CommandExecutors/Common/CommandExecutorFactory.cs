using System;
using Backend.CommandExecutors.Lobby;
using Backend.CommandExecutors.Player;
using Backend.CommandExecutors.Session;
using ENet;
using Shared.Commands.Common;
using Shared.Commands.Lobby;
using Shared.Commands.Player;
using Shared.Commands.Session;
using Shared.Protocol;

namespace Backend.CommandExecutors.Common
{
    public class CommandExecutorFactory
    {
        private readonly byte[] _buffer = new byte[1024];

        private readonly WorldModel _world;

        private int counter = 0;

        public CommandExecutorFactory(WorldModel world)
        {
            _world = world;
        }

        public ICommandExecutor CreateCommandExecutor(ref Event netEvent)
        {
            netEvent.Packet.CopyTo(_buffer);
            var eNetProtocol = new NetworkProtocol(_buffer);

            eNetProtocol.Get(out string commandName);

            if (commandName == CommandConst.Login)
            {
                return new LoginCommandExecutor(new LoginCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.Logout)
            {
                return new LogoutCommandExecutor(new LogoutCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.CreateLobby)
            {
                return new CreateLobbyCommandExecutor(new CreateLobbyCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.JoinLobby)
            {
                return new JoinLobbyCommandExecutor(new JoinLobbyCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.LeaveLobby)
            {
                return new LeaveLobbyCommandExecutor(new LeaveLobbyCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.MovePlayer)
            {
                return new MoveCommandExecutor(new MoveCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.StartSession)
            {
                return new StartSessionCommandExecutor(new StartSessionCommand(eNetProtocol), _world,
                    netEvent.Peer);
            }

            if (commandName == CommandConst.LeaveSession)
            {
                return new LeaveSessionCommandExecutor(new LeaveSessionCommand(eNetProtocol), _world,
                    netEvent.Peer);
            }

            if (commandName == CommandConst.RotatePlayer)
            {
                return new RotateCommandExecutor(new RotateCommand(eNetProtocol), _world, netEvent.Peer);
            }

            if (commandName == CommandConst.SwitchWeaponId)
            {
                return new SwitchWeaponCommandExecutor(new SwitchWeaponCommand(eNetProtocol), _world,
                    netEvent.Peer);
            }

            if (commandName == CommandConst.PlayerAttack)
            {
                counter++;
                Console.WriteLine($"{counter}");

                return new PlayerAttackCommandExecutor(new PlayerAttackCommand(eNetProtocol), _world,
                    netEvent.Peer);
            }

            return null;
        }
    }
}
