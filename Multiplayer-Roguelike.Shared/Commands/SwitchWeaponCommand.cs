using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands
{
    public class SwitchWeaponCommand : BaseCommand
    {
        public override string Id => CommandConst.SwitchWeaponId;

        public string PlayerId;
        public ushort WeaponId;

        public SwitchWeaponCommand(string playerId, ushort weaponId)
        {
            PlayerId = playerId;
            WeaponId = weaponId;
        }

        public SwitchWeaponCommand(NetworkProtocol protocol) : base(protocol)
        {

        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerId);
            protocol.Get(out WeaponId);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerId);
            protocol.Add(WeaponId);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
