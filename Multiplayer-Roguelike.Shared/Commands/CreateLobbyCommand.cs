using ENet;

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

        public CreateLobbyCommand(ENetProtocol protocol) : base(protocol)
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