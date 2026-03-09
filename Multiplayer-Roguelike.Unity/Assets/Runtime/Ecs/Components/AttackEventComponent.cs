namespace Runtime.ECS.Components
{
    public class AttackEventComponent : IComponent
    {
        public ushort AttackerId;
        public ushort TargetId;

        public AttackEventComponent(ushort attackerId, ushort targetId)
        {
            TargetId = targetId;
            AttackerId = attackerId;
        }
    }
}
