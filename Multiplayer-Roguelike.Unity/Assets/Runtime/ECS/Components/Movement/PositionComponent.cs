using UnityEngine;

namespace Runtime.ECS.Components.Movement
{
    public class PositionComponent : IComponent
    {
        public Vector3 Position;

        public PositionComponent(Vector3 position)
        {
            Position = position;
        }
    }
}
