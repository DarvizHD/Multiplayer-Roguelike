using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands.Player
{
    public class PlayerAttackCommand : BaseCommand
    {
        public override string Id => CommandConst.PlayerAttack;

        public string PlayerId;
        public string TargetId;

        public PlayerAttackCommand(NetworkProtocol protocol) : base(protocol)
        {

        }

        public PlayerAttackCommand(string playerId, string targetId)
        {
            PlayerId = playerId;
            TargetId = targetId;
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerId);
            protocol.Get(out TargetId);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerId);
            protocol.Add(TargetId);

            var buffer = protocol.Stream.ToArray();
            packet.Create(buffer, buffer.Length, PacketFlags.Reliable);
            peer.Send(0, ref packet);
        }
    }
}
