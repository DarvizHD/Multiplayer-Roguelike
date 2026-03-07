using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands.Player
{
    public class LogoutCommand : BaseCommand
    {
        public override string Id => CommandConst.Logout;
        public string PlayerNickname;

        public LogoutCommand(string playerName)
        {
            PlayerNickname = playerName;
        }

        public LogoutCommand(NetworkProtocol networkProtocol) : base(networkProtocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerNickname);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
