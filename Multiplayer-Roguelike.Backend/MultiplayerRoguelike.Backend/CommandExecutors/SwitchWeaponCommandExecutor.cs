using System;
using Backend.CommandExecutors.Common;
using ENet;
using Shared.Commands;
using Shared.Commands.Player;

namespace Backend.CommandExecutors
{
    public class SwitchWeaponCommandExecutor : BaseCommandExecutor<SwitchWeaponCommand>
    {
        public SwitchWeaponCommandExecutor(SwitchWeaponCommand command, WorldModel world, Peer peer)
            : base(command, world, ref peer)
        {
        }

        public override void Execute()
        {
            if (!World.Players.TryGet(Command.PlayerId, out var player))
            {
                Console.WriteLine($"Player {Command.PlayerId} not found");
                return;
            }

            if (!World.Sessions.TryGet(player.SessionId, out var session))
            {
                Console.WriteLine($"Player {Command.PlayerId} has no session");
                return;
            }

            session.GameSessionSharedModel.Characters.TryGet(player.PlayerSharedModel.Id, out var character);
            character.EquippedWeaponSlotId.Value = Command.WeaponId;
        }
    }
}
