using System;
using Backend.CommandExecutors.Common;
using Backend.Enemies;
using ENet;
using Shared.Commands;
using Shared.Primitives;

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

            Random random = new();

            for (var i = 10; i < 10 + Command.Count; i++)
            {
                var randomPosition = new Vector3(random.Next(-10, 10), 0f, random.Next(-10, 10));
                var startHealth = 100f;

                var enemy = new EnemyModel(i);
                enemy.Shared.TargetPlayerId.Value = Command.TargetId;
                enemy.Shared.Position.Value = randomPosition;
                enemy.Shared.Health.Value = startHealth;
                sessionModel.GameSessionSharedModel.Characters.TryGet(Command.TargetId, out var target);
                sessionModel.GameSessionSharedModel.Enemies.Add(enemy.Shared);
                sessionModel.Enemies.Add(i, enemy);
            }
        }
    }
}
