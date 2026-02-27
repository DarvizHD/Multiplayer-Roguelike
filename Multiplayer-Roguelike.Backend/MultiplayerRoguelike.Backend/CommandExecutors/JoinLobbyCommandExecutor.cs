using System;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public class JoinLobbyCommandExecutor : BaseCommandExecutor<JoinLobbyCommand>
    {
        public JoinLobbyCommandExecutor(JoinLobbyCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Join lobby");

            if (!World.Lobbies.TryGet(Command.LobbyId, out var lobby))
            {
                Console.WriteLine($"Undefined lobby {Command.LobbyId}");
                return;
            }

            if (!World.Players.TryGet(Command.PlayerNickname,  out var player))
            {
                Console.WriteLine($"Undefined player {player.PlayerNickname}");
                return;
            }
            
            if (player.PartyId != string.Empty)
            {
                var previousLobby = World.Lobbies.Get(player.PartyId);
                previousLobby.RemoveMember(player.PlayerNickname);
            }
            
            lobby.AddMember(player.PlayerNickname);
        }
    }
}