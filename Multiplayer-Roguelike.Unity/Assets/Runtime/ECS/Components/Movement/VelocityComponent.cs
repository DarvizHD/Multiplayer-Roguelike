using UnityEngine;

namespace Runtime.ECS.Components.Movement
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