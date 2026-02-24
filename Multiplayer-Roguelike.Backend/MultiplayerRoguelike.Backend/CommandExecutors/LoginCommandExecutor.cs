using System;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors
{
    public class LoginCommandExecutor : BaseCommandExecutor<LoginCommand>
    {
        public LoginCommandExecutor(LoginCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
        {
            
        }

        public override void Execute()
        {
            Console.WriteLine($"Player {Command.PlayerNickname} wants to login");
        }
    }
}