namespace Runtime.ECS.Components.Battle
{
    public class AttackCooldownComponent : IComponent
    {
        public float Cooldown;

        public float CurrentCooldown;

        public AttackCooldownComponent(float cooldown)
        {
            Cooldown = cooldown;

            CurrentCooldown = cooldown;
        }
    }
}
