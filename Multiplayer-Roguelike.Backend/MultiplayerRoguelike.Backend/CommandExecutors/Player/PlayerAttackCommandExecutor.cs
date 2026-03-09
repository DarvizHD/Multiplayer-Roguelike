using System;
using Backend.CommandExecutors.Common;
using Backend.Constants;
using ENet;
using Shared.Commands;

namespace Backend.CommandExecutors.Player
{
    public class PlayerAttackCommandExecutor : BaseCommandExecutor<PlayerAttackCommand>
    {
        public PlayerAttackCommandExecutor(PlayerAttackCommand command, WorldModel world, Peer peer) : base(command, world, ref peer)
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

            if (!session.GameSessionSharedModel.Enemies.TryGet(Command.TargetId, out var target))
            {
                Console.WriteLine($"Enemy with {Command.TargetId} not found");
                return;
            }

            session.GameSessionSharedModel.Characters.TryGet(player.PlayerSharedModel.Id, out var character);

            if (target.Health.Value <= 0f)
            {
                return;
            }

            var weaponId = character.EquippedWeaponSlotId.Value;
            var damage = weaponId < WeaponConstants.Damages.Length ? WeaponConstants.Damages[character.EquippedWeaponSlotId.Value] : 10f;
            var time = DateTime.UtcNow.ToString("HH:mm:ss");
            var eventName = weaponId < WeaponConstants.Events.Length ? WeaponConstants.Events[character.EquippedWeaponSlotId.Value] : "default";
            character.EventId.Value = $"{eventName}_{time}";
            target.Health.Value -= damage;
        }
    }
}
