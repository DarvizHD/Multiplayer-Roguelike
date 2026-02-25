using System;
using Backend.Lobby;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public class CreateLobbyExecutor : BaseCommandExecutor<CreateLobbyCommand>
    {
        public CreateLobbyExecutor(CreateLobbyCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Create lobby");

            var player = World.Players.Get(Command.PlayerNickname);
            if (player.PartyId != string.Empty)
            {
                return;
            }

            var lobby = new LobbyModel(Guid.NewGuid().ToString(), player.PlayerNickname);
            World.Lobbies.Add(lobby.Guid, lobby);

            lobby.AddMember(player.PlayerNickname);
        }
    }
}
