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
            
            var player = World.Players.Get(Command.PlayerNickname);
            var lobby = World.Lobbies.Get(Command.LobbyId);
            
            lobby.AddMember(player.PlayerNickname);
        }
    }
}