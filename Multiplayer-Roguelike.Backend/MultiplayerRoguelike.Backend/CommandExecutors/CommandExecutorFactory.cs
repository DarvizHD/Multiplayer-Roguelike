using ENet;
using Shared.Commands;
using Shared.Protocol;

namespace Backend.CommandExecutors
{
    public class CommandExecutorFactory
    {
        private readonly byte[] _buffer = new byte[1024];

        private readonly WorldModel _world;

        public CommandExecutorFactory(WorldModel world)
        {
            _world = world;
        }

        public ICommandExecutor CreateCommandExecutor(ref Event netEvent)
        {
            netEvent.Packet.CopyTo(_buffer);
            var eNetProtocol = new NetworkProtocol(_buffer);

            eNetProtocol.Get(out string commandName);
            return commandName switch
            {
                CommandConst.Login => new LoginCommandExecutor(new LoginCommand(eNetProtocol), _world, netEvent.Peer),
                CommandConst.CreateLobby => new CreateLobbyCommandExecutor(new CreateLobbyCommand(eNetProtocol), _world, netEvent.Peer),
                CommandConst.JoinLobby => new JoinLobbyCommandExecutor(new JoinLobbyCommand(eNetProtocol), _world, netEvent.Peer),
                CommandConst.MovePlayer => new MoveCommandExecutor(new MoveCommand(eNetProtocol), _world, netEvent.Peer),
                _ => null
            };
        }
    }
}
