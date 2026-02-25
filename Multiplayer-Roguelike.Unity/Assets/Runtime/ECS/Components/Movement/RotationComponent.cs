using UnityEngine;

namespace Runtime.ECS.Components.Movement
{
    public class RotationComponent : IComponent
    {
        public Quaternion Rotation;
        
        public RotationComponent(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
}