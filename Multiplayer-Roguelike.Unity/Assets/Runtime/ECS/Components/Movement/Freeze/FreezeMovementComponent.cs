namespace Runtime.ECS.Components
{
    public class FreezeMovementComponent : IComponent
    {
        public readonly float CachedMoveSpeedSpeed;
        public readonly float CachedRotationSpeed;
        public float Duration;
        public float CurrentDuration;

        public FreezeMovementComponent(float cachedMoveSpeedSpeed, float cachedRotationSpeed, float duration)
        {
            CachedMoveSpeedSpeed = cachedMoveSpeedSpeed;
            Duration = duration;
            CachedRotationSpeed = cachedRotationSpeed;
            CurrentDuration = duration;
        }
    }
}
