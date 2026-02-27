using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public interface ICommand
    {
        void Read(NetworkProtocol protocol);
        void Write(Peer peer);
    }
}
