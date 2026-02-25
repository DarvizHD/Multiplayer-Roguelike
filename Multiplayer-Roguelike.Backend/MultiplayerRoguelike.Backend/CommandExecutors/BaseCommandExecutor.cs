using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public abstract class BaseCommandExecutor<T> : ICommandExecutor where T : BaseCommand
    {
        protected readonly T Command;
        protected readonly WorldModel World;
        protected readonly Peer Peer;

        public BaseCommandExecutor(T command, WorldModel world, ref Peer peer)
        {
            Command = command;
            World = world;
            Peer = peer;
        }

        public abstract void Execute();
    }
}
