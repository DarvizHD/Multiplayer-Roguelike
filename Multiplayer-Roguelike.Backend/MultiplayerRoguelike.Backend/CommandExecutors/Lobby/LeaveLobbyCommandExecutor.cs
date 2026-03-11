using System;
using Backend.CommandExecutors.Common;
using ENet;
using Shared.Commands.Lobby;

namespace Backend.CommandExecutors.Lobby
{
    public class LeaveLobbyCommandExecutor : BaseCommandExecutor<LeaveLobbyCommand>
    {
        public LeaveLobbyCommandExecutor(LeaveLobbyCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            if (!World.Lobbies.TryGet(Command.LobbyId, out var lobby))
            {
                Console.WriteLine($"Undefined lobby {Command.LobbyId}");
                return;
            }

            if (!World.Players.TryGet(Command.PlayerNickname, out var player))
            {
                Console.WriteLine($"Undefined player {player.PlayerSharedModel.Nickname.Value}");
                return;
            }

            var previousLobby = World.Lobbies.Get(player.PlayerSharedModel.Lobby.LobbyId.Value);
            previousLobby.RemoveMember(player.PlayerSharedModel.Nickname.Value);
        }
    }
}
