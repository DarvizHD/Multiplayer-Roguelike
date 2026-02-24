using ENet;
using Shared.Commands;

namespace Server.CommandExecutors
{
    public abstract class BaseCommandExecutor<T> : ICommandExecutor where T : BaseCommand
    {
        protected readonly T Command;
        protected readonly Peer Peer;

        public BaseCommandExecutor(T command, ref Peer peer)
        {
            Command = command;
            Peer = peer;
        }

        public abstract void Execute();
    }
}