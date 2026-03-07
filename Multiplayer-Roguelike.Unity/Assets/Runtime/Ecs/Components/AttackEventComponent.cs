namespace Runtime.Ecs.Components
{
    public class AttackEventComponent : IComponent
    {
        public ushort TargetId;
        public float Damage;

        public AttackEventComponent(ushort targetId, float damage)
        {
            TargetId = targetId;
            Damage = damage;
        }
    }
}
