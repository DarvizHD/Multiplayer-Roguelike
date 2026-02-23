using ENet;
using Server.CommandExecutors;

namespace Server.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract string Id { get; }
        
        protected BaseCommand(ENetProtocol protocol)
        {
            Read(protocol);
        }
        
        protected BaseCommand() { }
        
        public abstract void Read(ENetProtocol protocol);

        public abstract void Write(Peer peer);
    }
}