using System;
using Backend.CommandExecutors.Common;
using ENet;
using Shared.Commands.Session;

namespace Backend.CommandExecutors.Session
{
    public class LeaveSessionCommandExecutor : BaseCommandExecutor<LeaveSessionCommand>
    {
        public LeaveSessionCommandExecutor(LeaveSessionCommand command, WorldModel world, Peer peer) : base(command,
            world, ref peer)
        {
        }

        public override void Execute()
        {
            if (!World.Players.TryGet(Command.PlayerNickname, out var player))
            {
                Console.WriteLine($"Player {Command.PlayerNickname} not found");
                return;
            }

            if (!World.Sessions.TryGet(player.PlayerSharedModel.Lobby.LobbyId.Value, out var session))
            {
                Console.WriteLine($"Session {Command.SessionId} not found");
                return;
            }

            if (!session.Players.Models.TryGetValue(Command.PlayerNickname, out var playerModel))
            {
                Console.WriteLine($"There is no player {Command.PlayerNickname} in session {session.Id}");
                return;
            }

            session.Players.Remove(playerModel.PlayerSharedModel.Id);
        }
    }
}
