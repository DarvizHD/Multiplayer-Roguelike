namespace Runtime.ECS.Components.Player
{
    public class PlayerLookRotationComponent : IComponent
    {
        public float RotationSpeed;

        public PlayerLookRotationComponent(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }
    }
}