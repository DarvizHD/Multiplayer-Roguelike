using UnityEngine;

namespace Runtime.ECS.Components.Movement
{
    public class DirectionComponent : IComponent
    {
        public Vector3 Direction;
        public Vector3 AvoidanceVelocity;
        public Vector3 DesiredDirection;
        
        public DirectionComponent(Vector3 direction)
        {
            Direction = direction;
        }
    }
}