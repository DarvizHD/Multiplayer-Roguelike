namespace Runtime.Ecs.Components.Movement.Freeze
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
