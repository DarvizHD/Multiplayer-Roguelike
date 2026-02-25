using ENet;

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

        public JoinLobbyCommand(ENetProtocol protocol) : base(protocol)
        {
            
        }
        
        public override void Read(ENetProtocol protocol)
        {
            protocol.Get(out LobbyId);
            protocol.Get(out PlayerNickname);
        }

        public override void Write(Peer peer)
        {
            var protocol = new ENetProtocol();
            var packet = default(Packet);
            
            protocol.Add(Id);
            protocol.Add(LobbyId);
            protocol.Add(PlayerNickname);
            
            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}