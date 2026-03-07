using Runtime.Ecs.Components;
using UnityEngine;

namespace Runtime.Ecs.Systems.Movement
{
    public class RigidbodyComponent : IComponent
    {
        public Rigidbody Rigidbody;

        public RigidbodyComponent(Rigidbody rigidbody)
        {
            Rigidbody = rigidbody;
        }
    }
}
