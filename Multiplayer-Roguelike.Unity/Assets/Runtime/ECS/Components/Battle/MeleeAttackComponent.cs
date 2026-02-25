namespace Runtime.ECS.Components.Battle
{
    public class MeleeAttackComponent : IComponent
    {
        public float Range;

        public float Damage;

        public MeleeAttackComponent(float range, float damage)
        {
            Range = range;
            Damage = damage;
        }
    }
}
