namespace Runtime.ECS.Components.Health
{
    public class DeathAnimationComponent : IComponent
    {
        public float AnimationDuration;
        public float Timer;

        public DeathAnimationComponent(float duration)
        {
            AnimationDuration = duration;
            Timer = duration;
        }
    }
}
