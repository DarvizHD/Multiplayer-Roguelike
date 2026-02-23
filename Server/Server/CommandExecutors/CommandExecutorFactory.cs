using ENet;
using Server.Commands;

namespace Server.CommandExecutors
{
    public class CommandExecutorFactory
    {
        private readonly byte[] _buffer = new byte[1024];
        
        public ICommandExecutor CreateCommandExecutor(ref Event netEvent)
        {
            netEvent.Packet.CopyTo(_buffer);
            var eNetProtocol = new ENetProtocol(_buffer);
            
            eNetProtocol.Get(out string commandName);
            return commandName switch
            {
                CommandConst.Login => new LoginCommandExecutor(new LoginCommand(eNetProtocol), netEvent.Peer),
                _ => null
            };
        }
    }
}