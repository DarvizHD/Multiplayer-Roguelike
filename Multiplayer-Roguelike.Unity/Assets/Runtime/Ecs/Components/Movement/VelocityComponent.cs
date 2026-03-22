using UnityEngine;

namespace Runtime.Ecs.Components.Movement
{
    public class VelocityComponent : IComponent
    {
        public Vector3 Velocity;

        public VelocityComponent(Vector3 velocity)
        {
            Velocity = velocity;
        }
    }
}
