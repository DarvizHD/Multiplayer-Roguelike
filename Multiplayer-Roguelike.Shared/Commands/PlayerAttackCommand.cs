using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands
{
    public class PlayerAttackCommand : BaseCommand
    {
        public override string Id => CommandConst.PlayerAttack;

        public string PlayerId;
        public string TargetId;
        public string EventId;

        public PlayerAttackCommand(NetworkProtocol protocol) : base(protocol)
        {

        }

        public PlayerAttackCommand(string playerId, string targetId, string eventId)
        {
            PlayerId = playerId;
            TargetId = targetId;
            EventId = eventId;
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerId);
            protocol.Get(out TargetId);
            protocol.Get(out EventId);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerId);
            protocol.Add(TargetId);
            protocol.Add(EventId);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
