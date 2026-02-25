using ENet;
using Shared;
using Shared.Commands;

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
            var eNetProtocol = new ENetProtocol(_buffer);

            eNetProtocol.Get(out string commandName);
            return commandName switch
            {
                CommandConst.Login => new LoginCommandExecutor(new LoginCommand(eNetProtocol), _world, netEvent.Peer),
                CommandConst.CreateLobby => new CreateLobbyExecutor(new CreateLobbyCommand(eNetProtocol), _world, netEvent.Peer),
                _ => null
            };
        }
    }
}
