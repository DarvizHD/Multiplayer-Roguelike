namespace Runtime.ECS.Components.Health
{
    public class HealthComponent : IComponent
    {
        public float CurrentHealth;
        public float MaxHealth;
        
        public HealthComponent(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}