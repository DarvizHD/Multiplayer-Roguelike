using ENet;
using Shared.Protocol;

namespace Shared.Commands.Common
{
    public interface ICommand
    {
        void Read(NetworkProtocol protocol);
        void Write(Peer peer);
    }
}
