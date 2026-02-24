using System;
using ENet;
using Shared.Commands;

namespace Multiplayer
{
    public class LoginCommandExecutor : BaseCommandExecutor<LoginCommand>
    {
        public LoginCommandExecutor(LoginCommand command, Peer peer) : base(command, ref peer)
        {
            
        }

        public override void Execute()
        {
            Console.WriteLine($"Player {Command.PlayerNickname} wants to login");
        }
    }
}