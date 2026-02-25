namespace Runtime.ECS.Components.Health
{
    public class DeathComponent : IComponent
    {
        public bool IsDead;
        
        public DeathComponent()
        {
            IsDead = true;
        }
    }
}