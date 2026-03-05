using System;
using ENet;

namespace Backend.CommandExecutors
{
    public class RotateCommandExecutor : BaseCommandExecutor<RotateCommand>
    {
        public RotateCommandExecutor(RotateCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {

        }

        public override void Execute()
        {
            if (!World.Players.TryGet(Command.PlayerNickname, out var player))
            {
                Console.WriteLine($"Player {Command.PlayerNickname} not found");
                return;
            }

            if (!World.Sessions.TryGet(player.SessionId, out var session))
            {
                Console.WriteLine($"Player {Command.PlayerNickname} has no session");
                return;
            }

            session.GameSessionSharedModel.Characters.TryGet(player.PlayerSharedModel.Id, out var character);
            character.Rotation.Value = Command.Rotation;
        }
    }
}
