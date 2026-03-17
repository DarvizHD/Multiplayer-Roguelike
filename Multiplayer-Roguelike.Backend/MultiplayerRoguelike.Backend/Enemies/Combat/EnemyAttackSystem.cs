using System;
using Backend.ServerSystems;
using Backend.Session;
using Shared.Models.Player;

namespace Backend.Enemies.Combat
{
    public class EnemyAttackSystem : IServerSystem
    {
        private const int _tickRate = 32;
        private const float _tickInterval = 1f / _tickRate;

        public string Id { get; }

        private GameSessionModel GameSessionModel { get; }
        private float _timer;

        public EnemyAttackSystem(string id, GameSessionModel gameSessionModel)
        {
            Id = id;
            GameSessionModel = gameSessionModel;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer > _tickInterval)
            {
                _timer = 0f;

                foreach (var enemy in GameSessionModel.Enemies.Models.Values)
                {
                    UpdateEnemy(enemy, deltaTime);
                }
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

            if (enemy.Shared.TargetPlayerId.Value != string.Empty)
            {
                if (GameSessionModel.SharedModel.Characters.TryGet(enemy.Shared.TargetPlayerId.Value, out var targetPlayer) && targetPlayer.Health.Value > 0)
                {
                    var distance = (enemy.Position - targetPlayer.Position.Value).LengthSquared();
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

            var time = DateTime.UtcNow.ToString("HH:mm:ss");
            player.EventId.Value = $"damage_{time}";

            enemy.Shared.AnimationState.Value = $"attack_{time}";

            attack.CooldownTimer = enemy.EnemyAttack.Cooldown;
        }
    }
}
