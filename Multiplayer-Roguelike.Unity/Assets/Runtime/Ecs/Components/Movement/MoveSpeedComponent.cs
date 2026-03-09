namespace Runtime.ECS.Components.Movement
{
    public class MoveSpeedComponent : IComponent
    {
        public float Speed;

        public MoveSpeedComponent(float speed)
        {
            Speed = speed;
        }
    }
}
