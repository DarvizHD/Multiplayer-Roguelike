using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands.Lobby
{
    public class LeaveLobbyCommand : BaseCommand
    {
        public override string Id => CommandConst.LeaveLobby;

        public string LobbyId;
        public string PlayerNickname;

        public LeaveLobbyCommand(string playerNickname, string lobbyId)
        {
            LobbyId = lobbyId;
            PlayerNickname = playerNickname;
        }

        public LeaveLobbyCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out LobbyId);
            protocol.Get(out PlayerNickname);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(LobbyId);
            protocol.Add(PlayerNickname);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
