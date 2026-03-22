using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands.Lobby
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
            var buffer = protocol.Stream.ToArray();
            packet.Create(buffer, buffer.Length, PacketFlags.Reliable);
            peer.Send(0, ref packet);
        }
    }
}
