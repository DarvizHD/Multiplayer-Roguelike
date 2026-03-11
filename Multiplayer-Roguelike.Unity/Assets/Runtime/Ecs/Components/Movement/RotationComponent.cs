namespace Runtime.ECS.Components.Movement
{
    public class RotationComponent : IComponent
    {
        public float Angle;

        public RotationComponent(float angle = 0f)
        {
            Angle = angle;
        }
    }
}
