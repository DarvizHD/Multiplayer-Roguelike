using UnityEngine;

namespace Runtime.Ecs.Components.Movement
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
