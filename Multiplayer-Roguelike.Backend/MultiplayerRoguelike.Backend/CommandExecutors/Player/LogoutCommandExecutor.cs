using System;
using Backend.CommandExecutors.Common;
using ENet;
using Shared.Commands.Player;

namespace Backend.CommandExecutors.Player
{
    public class LogoutCommandExecutor : BaseCommandExecutor<LogoutCommand>
    {
        public LogoutCommandExecutor(LogoutCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            if (!World.Players.TryGet(Command.PlayerNickname, out var existedPlayer))
            {
                Console.WriteLine($"Player with name {Command.PlayerNickname} there is not in game");
                return;
            }

            if (!World.Sessions.TryGet(existedPlayer.SessionId, out var session))
            {
                World.Players.Remove(existedPlayer.PlayerSharedModel.Id);
            }
            existedPlayer.Peer.Disconnect(0);
        }
    }
}
