using UnityEngine;

namespace Runtime.Ecs.Components
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
