using ENet;

namespace Shared.Commands
{
    public class LoginCommand : BaseCommand
    {
        public override string Id => CommandConst.Login;
        public string PlayerNickname;
        
        public LoginCommand(string playerNickname)
        {
            PlayerNickname = playerNickname;
        }
        
        public LoginCommand(ENetProtocol protocol) : base(protocol)
        {
            
        }

        public override void Read(ENetProtocol protocol)
        {
            protocol.Get(out PlayerNickname);
        }

        public override void Write(Peer peer)
        {
            var protocol = new ENetProtocol();
            var packet = default(Packet);
            
            protocol.Add(Id);
            protocol.Add(PlayerNickname);
            
            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}