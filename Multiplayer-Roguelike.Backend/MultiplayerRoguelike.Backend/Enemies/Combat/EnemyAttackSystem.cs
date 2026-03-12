using System;
using Backend.ServerSystems;
using Backend.Session;
using Shared.Models.Player;

namespace Backend.Enemies.Combat
{
    public class EnemyAttackSystem : IServerSystem
    {
        public string Id { get; }

        private SessionModel SessionModel { get; }

        public EnemyAttackSystem(string id, SessionModel sessionModel)
        {
            Id = id;
            SessionModel = sessionModel;
        }

        public void Update(float deltaTime)
        {
            foreach (var enemy in SessionModel.Enemies.Models.Values)
            {
                UpdateEnemy(enemy, deltaTime);
            }
        }

        private void UpdateEnemy(EnemyModel enemy, float deltaTime)
        {
            var attack = enemy.EnemyAttack;

            if (attack.CooldownTimer > 0)
            {
                attack.CooldownTimer -= deltaTime;
                return;
            }

            if (enemy.Shared.TargetPlayerId.Value != null)
            {
                if (SessionModel.GameSessionSharedModel.Characters.TryGet(enemy.Shared.TargetPlayerId.Value, out var targetPlayer) && targetPlayer.Health.Value > 0)
                {
                    var distance = (enemy.Shared.Position.Value - targetPlayer.Position.Value).LengthSquared();
                    if (!(distance > attack.Range * attack.Range))
                    {
                        PerformAttack(enemy, targetPlayer);
                    }
                }
            }
        }

        private void PerformAttack(EnemyModel enemy, CharacterSharedModel player)
        {
            var attack = enemy.EnemyAttack;

            player.Health.Value -= attack.Damage;

            attack.CooldownTimer = enemy.EnemyAttack.Cooldown;
        }
    }
}
