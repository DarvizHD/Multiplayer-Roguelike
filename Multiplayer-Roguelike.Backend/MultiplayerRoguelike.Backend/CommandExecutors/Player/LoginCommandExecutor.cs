using System;
using System.Linq;
using Backend.CommandExecutors.Common;
using Backend.Player;
using ENet;
using Shared.Commands.Player;

namespace Backend.CommandExecutors.Player
{
    public class LoginCommandExecutor : BaseCommandExecutor<LoginCommand>
    {
        public LoginCommandExecutor(LoginCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            Console.WriteLine($"Player {Command.PlayerNickname} wants to login");

            if (World.Players.TryGet(Command.PlayerNickname, out var existedPlayer))
            {
                if (!existedPlayer.IsActive && World.Sessions.TryGet(existedPlayer.SessionId, out var session))
                {
                    existedPlayer.Peer = Peer;
                    existedPlayer.IsActive = true;
                    existedPlayer.IsConnectingToSession = true;
                }
                else
                {
                    Console.WriteLine($"Player with name {existedPlayer.PlayerSharedModel.Nickname.Value} has already been logged in");
                }
                return;
            }

            if (World.Players.Models.Values.Any(p => p.Peer.ID == Peer.ID))
            {
                Console.WriteLine($"Player on Peer {Peer.ID} has already been logged in");
                return;
            }

            var player = new PlayerModel(Command.PlayerNickname, Peer);
            World.Players.Add(player.PlayerSharedModel.Nickname.Value, player);
        }
    }
}
