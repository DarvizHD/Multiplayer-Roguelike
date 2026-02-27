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
                Console.WriteLine($"Undefined player {player.PlayerNickname}");
                return;
            }
            
            if (player.PartyId != string.Empty)
            {
                Console.WriteLine($"Lobby {player.PartyId} already exists");
                return;
            }

            var lobby = new LobbyModel(Guid.NewGuid().ToString(), player.PlayerNickname);
            World.Lobbies.Add(lobby.Guid, lobby);
            
            lobby.AddMember(player.PlayerNickname);
        }
    }
}
