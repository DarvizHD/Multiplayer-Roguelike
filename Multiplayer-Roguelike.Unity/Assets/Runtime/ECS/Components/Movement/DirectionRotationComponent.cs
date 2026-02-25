namespace Runtime.ECS.Components.Movement
{
    public class DirectionRotationComponent : IComponent
    {
        public float RotationSpeed;

        public DirectionRotationComponent(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }
    }
}