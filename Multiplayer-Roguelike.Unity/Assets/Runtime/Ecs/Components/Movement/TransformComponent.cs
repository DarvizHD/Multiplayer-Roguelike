using UnityEngine;

namespace Runtime.Ecs.Components.Movement
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
