namespace Runtime.Components
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