using ENet;
using Shared.Commands.Common;
using Shared.Primitives;
using Shared.Protocol;

namespace Shared.Commands.Player
{
    public class MoveCommand : BaseCommand
    {
        public override string Id => CommandConst.MovePlayer;

        public string PlayerNickname;
        public Vector3 Direction;
        public Vector3 Position;

        public MoveCommand(string playerNickname, Vector3 position, Vector3 direction)
        {
            PlayerNickname = playerNickname;
            Position = position;
            Direction = direction;
        }

        public MoveCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
            protocol.Get(out Position);
            protocol.Get(out Direction);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerNickname);
            protocol.Add(Position);
            protocol.Add(Direction);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
