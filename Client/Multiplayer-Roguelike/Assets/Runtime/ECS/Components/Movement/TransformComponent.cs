using UnityEngine;

namespace Runtime.Components
{
    public class TransformComponent : IComponent
    {
        public Transform Transform { get; }
        
        public TransformComponent(Transform transform)
        {
            Transform = transform;
        }
    }
}