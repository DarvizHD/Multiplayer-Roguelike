using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public class RotateCommand : BaseCommand
    {
        public override string Id => CommandConst.RotatePlayer;

        public string PlayerNickname;

        public float Rotation;

        public RotateCommand(string playerNickname, float rotation)
        {
            PlayerNickname = playerNickname;
            Rotation = rotation;
        }


        public RotateCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
            protocol.Get(out Rotation);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerNickname);
            protocol.Add(Rotation);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
