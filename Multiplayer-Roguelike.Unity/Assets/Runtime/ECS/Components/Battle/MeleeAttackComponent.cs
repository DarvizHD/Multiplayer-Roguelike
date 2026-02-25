namespace Runtime.ECS.Components.Battle
{
    public class MeleeAttackComponent : IComponent
    {
        public float Range {get; set;}
        public float Angle { get; set; }
        public float Damage {get; set;} 

        public MeleeAttackComponent(float range, float damage, float angle = 90f)
        {
            Range = range;
            Damage = damage;
            Angle = angle;
        }
    }
}