using UnityEngine;

namespace Runtime.Components
{
    public class DirectionComponent : IComponent
    {
        public Vector3 Direction;
        
        public DirectionComponent(Vector3 direction)
        {
            Direction = direction;
        }
    }
}