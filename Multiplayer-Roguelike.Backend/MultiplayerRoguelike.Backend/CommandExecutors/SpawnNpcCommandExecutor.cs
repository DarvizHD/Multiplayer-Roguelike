using System;
using ENet;
using Shared.Models;

namespace Backend.CommandExecutors
{
    public class SpawnNpcCommandExecutor : BaseCommandExecutor<SpawnNpcCommand>
    {
        public SpawnNpcCommandExecutor(SpawnNpcCommand command, WorldModel world, Peer peer)
            : base(command, world, ref peer)
        {

        }

        public override void Execute()
        {
            if (!World.Sessions.TryGet(Command.SessionId, out var sessionModel))
            {
                Console.WriteLine($"Session {Command.SessionId} not found");
                return;
            }

            var newNpc = new NpcSharedModel(Command.NpcId);

            newNpc.LastPosition.Value = Command.Position;
            newNpc.Health.Value = Command.Health;

            sessionModel.GameSessionSharedModel.NPCs.Add(newNpc);

            Console.WriteLine($"Spawn Npc {Command.NpcId} with {Command.Health} health in {Command.Position}");
        }
    }
}
