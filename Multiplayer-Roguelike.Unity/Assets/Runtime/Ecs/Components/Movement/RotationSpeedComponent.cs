namespace Runtime.Ecs.Components.Movement
{
    public class RotationSpeedComponent : IComponent
    {
        public float Speed;

        public RotationSpeedComponent(float speed)
        {
            Speed = speed;
        }
    }
}
