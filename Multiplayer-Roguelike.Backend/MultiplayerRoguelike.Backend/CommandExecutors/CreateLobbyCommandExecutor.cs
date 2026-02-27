using System;
using Backend.Lobby;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public class CreateLobbyCommandExecutor : BaseCommandExecutor<CreateLobbyCommand>
    {
        public CreateLobbyCommandExecutor(CreateLobbyCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Create lobby");

            if (!World.Players.TryGet(Command.PlayerNickname,  out var player))
            {
                Console.WriteLine($"Undefined player {player.PlayerSharedModel.Nickname.Value}");
                return;
            }

            if (player.PlayerSharedModel.Lobby.LobbyId.Value != string.Empty)
            {
                Console.WriteLine($"Lobby {player.PlayerSharedModel.Lobby.LobbyId.Value} already exists");
                return;
            }

            var lobby = new LobbyModel(Guid.NewGuid().ToString(), player.PlayerSharedModel.Nickname.Value);
            World.Lobbies.Add(lobby.Guid, lobby);

            lobby.AddMember(player.PlayerSharedModel.Nickname.Value);
        }
    }
}
