using ENet;
using Shared.Commands.Common;
using Shared.Protocol;

namespace Shared.Commands
{
    public class SpawnNpcCommand : BaseCommand
    {
        public override string Id => CommandConst.SpawnNpc;

        public int Count;

        public string SessionId;

        public SpawnNpcCommand(string sessionId, int count)
        {
            SessionId = sessionId;
            Count = count;
        }

        public SpawnNpcCommand(NetworkProtocol protocol) : base(protocol)
        {
        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out SessionId);
            protocol.Get(out Count);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(SessionId);
            protocol.Add(Count);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
