namespace Runtime.ECS.Components.Movement
{
    public class SpeedComponent : IComponent
    {
        public float Speed;

        public SpeedComponent(float speed)
        {
            Speed = speed;
        }
    }
}
