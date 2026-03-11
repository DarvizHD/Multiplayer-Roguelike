namespace Backend.Enemies.Combat
{
    public class EnemyAttackModel
    {
        public float Range { get; set; }
        public float Damage { get; set; }
        public float Cooldown { get; set; }

        public float CooldownTimer { get; set; }
    }
}
