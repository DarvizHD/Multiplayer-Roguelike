namespace Runtime.ECS.Components
{
    public class FreezeMovementByDamageComponent : IComponent
    {
        public readonly float Duration;

        public FreezeMovementByDamageComponent(float duration)
        {
            Duration = duration;
        }
    }
}