using ENet;
using Server.CommandExecutors;

namespace Server.Commands
{
    public interface ICommand
    {
        void Read(ENetProtocol protocol);
        void Write(Peer peer);
    }
}