using UnityEngine;

namespace Runtime.ECS.Components
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
