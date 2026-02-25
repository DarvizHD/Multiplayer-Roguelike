using UnityEngine;

namespace Runtime.ECS.Components.Movement
{
    public class FollowComponent : IComponent
    {
        public Transform Target { get; }
        
        public FollowComponent(Transform target)
        {
            Target = target;
        }
    }
}