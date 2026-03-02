using ENet;
using Shared.Protocol;

namespace Shared.Commands
{
    public class StartSessionCommand : BaseCommand
    {
        public override string Id => CommandConst.StartSession;

        public string PlayerNickname;
        public string LobbyId;

        public StartSessionCommand(string playerNickname, string lobbyId)
        {
            PlayerNickname = playerNickname;
            LobbyId = lobbyId;
        }

        public StartSessionCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
            protocol.Get(out LobbyId);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(PlayerNickname);
            protocol.Add(LobbyId);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
