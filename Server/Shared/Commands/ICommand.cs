using ENet;

namespace Shared.Commands
{
    public interface ICommand
    {
        void Read(ENetProtocol protocol);
        void Write(Peer peer);
    }
}