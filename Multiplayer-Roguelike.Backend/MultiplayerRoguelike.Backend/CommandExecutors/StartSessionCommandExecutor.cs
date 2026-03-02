using System;
using Backend.Session;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public class StartSessionCommandExecutor : BaseCommandExecutor<StartSessionCommand>
    {
        public StartSessionCommandExecutor(StartSessionCommand command, WorldModel world, Peer peer) : base(command,
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

            if (!World.Lobbies.TryGet(player.PlayerSharedModel.Lobby.LobbyId.Value, out var lobby))
            {
                Console.WriteLine($"Player {Command.LobbyId} not found");
                return;
            }

            if (lobby.OwnerNickname != player.PlayerSharedModel.Nickname.Value)
            {
                Console.WriteLine($"Player {Command.PlayerNickname} are not owner of lobby {Command.LobbyId}");
                return;
            }

            if (player.SessionId != string.Empty)
            {
                World.Sessions.TryGet(player.SessionId, out var session);
                session.Players.Remove(player.PlayerSharedModel.Id);
            }

            var newSession = new SessionModel(lobby.Guid);
            World.Sessions.Add(newSession.Id, newSession);
            foreach (var memberId in lobby.Members)
            {
                World.Players.TryGet(memberId, out var member);
                newSession.Players.Add(memberId, member);
            }
        }
    }
}
