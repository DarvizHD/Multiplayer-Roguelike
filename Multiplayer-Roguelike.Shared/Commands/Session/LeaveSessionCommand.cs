using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands.Session
{
    public class LeaveSessionCommand : BaseCommand
    {
        public override string Id => CommandConst.LeaveSession;

        public string PlayerNickname;
        public string SessionId;

        public LeaveSessionCommand(string playerNickname, string sessionId)
        {
            PlayerNickname = playerNickname;
            SessionId = sessionId;
        }

        public LeaveSessionCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
            protocol.Get(out SessionId);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerNickname);
            protocol.Add(SessionId);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
