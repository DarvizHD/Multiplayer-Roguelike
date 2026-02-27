using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public class CreateLobbyCommand : BaseCommand
    {
        public override string Id => CommandConst.CreateLobby;
        public string PlayerNickname;

        public CreateLobbyCommand(string playerNickname)
        {
            PlayerNickname = playerNickname;
        }

        public CreateLobbyCommand(NetworkProtocol protocol) : base(protocol)
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
