using Backend.Enemies.Combat;
using DotRecast.Detour.Crowd;
using Shared.Models.Enemy;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyModel
    {
        public int Id { get; }
        public EnemySharedModel Shared { get; }
        public DtCrowdAgent CrowdAgent { get; set; }
        public Vector3 LastTargetPosition { get; set; } = new(0, 0, 0);
        public EnemyAttackModel EnemyAttack { get; }

        public EnemyModel(int id, EnemyConfig enemyConfig)
        {
            Id = id;
            Shared = new EnemySharedModel(id.ToString());

            EnemyAttack = new EnemyAttackModel
            {
                Range = enemyConfig.AttackRange,
                Damage = enemyConfig.AttackDamage,
                Cooldown = enemyConfig.AttackCooldown
            };
        }
    }
}
