using System;
using ENet;
using Shared.Commands;
using Shared.Primitives;
using Shared.Protocol;

namespace Backend.CommandExecutors
{
    public class SpawnNpcCommand : BaseCommand
    {
        public override string Id => CommandConst.SpawnNpc;

        public string NpcId;

        public Vector3 Position;

        public float Health;

        public string SessionId;

        public SpawnNpcCommand(string npcId, string sessionId, Vector3 position, float health)
        {
            NpcId = npcId;
            Position = position;
            Health = health;
            SessionId =  sessionId;
        }

        public SpawnNpcCommand(NetworkProtocol protocol) : base(protocol)
        {

        }

        public override void Read(NetworkProtocol protocol)
        {
            protocol.Get(out NpcId);
            protocol.Get(out SessionId);
            protocol.Get(out Position);
            protocol.Get(out Health);
        }

        public override void Write(Peer peer)
        {
            var protocol = new NetworkProtocol();
            var packet = default(Packet);

            protocol.Add(Id);
            protocol.Add(NpcId);
            protocol.Add(SessionId);
            protocol.Add(Position);
            protocol.Add(Health);

            packet.Create(protocol.Stream.GetBuffer());
            peer.Send(0, ref packet);
        }
    }
}
