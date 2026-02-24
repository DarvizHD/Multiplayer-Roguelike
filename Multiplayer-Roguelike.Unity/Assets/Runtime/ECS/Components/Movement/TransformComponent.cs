using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components.Movement
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