using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public abstract string Id { get; }

        protected BaseCommand(NetworkProtocol protocol)
        {
            Read(protocol);
        }

        protected BaseCommand()
        {
        }

        public abstract void Read(NetworkProtocol protocol);

        public abstract void Write(Peer peer);
    }
}
