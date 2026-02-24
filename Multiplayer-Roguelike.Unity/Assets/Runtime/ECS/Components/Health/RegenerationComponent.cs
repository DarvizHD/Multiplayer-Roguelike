namespace Runtime.ECS.Components.Health
{
    public class RegenerationComponent : IComponent
    {
        public float RegenerationRate;
        public float Cooldown;
        public float LastDamageTime;
        
        public RegenerationComponent(float rate, float cooldown)
        {
            RegenerationRate = rate;
            Cooldown = cooldown;
            LastDamageTime = 0f;
        }
    }
}