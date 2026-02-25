namespace Runtime.ECS.Components
{
    public class AttackEventComponent : IComponent
    {
        public int TargetId;
        public float Damage;

        public AttackEventComponent(int targetId, float damage)
        {
            TargetId = targetId;
            Damage = damage;
        }
    }
}