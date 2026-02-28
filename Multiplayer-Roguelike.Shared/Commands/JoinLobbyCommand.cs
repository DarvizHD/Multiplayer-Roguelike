using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public class JoinLobbyCommand : BaseCommand
    {
        public override string Id => CommandConst.JoinLobby;
        public string LobbyId;
        public string PlayerNickname;

        public JoinLobbyCommand(string lobbyId, string playerNickname)
        {
            LobbyId = lobbyId;
            PlayerNickname = playerNickname;
        }

        public JoinLobbyCommand(NetworkProtocol protocol) : base(protocol)
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
