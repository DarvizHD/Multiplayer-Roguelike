using UnityEngine;

namespace Runtime.Ecs.Components
{
    public class RigidbodyComponent : IComponent
    {
        public Rigidbody Rigidbody;

        public RigidbodyComponent(Rigidbody rigidbody, Vector3 position)
        {
            Rigidbody = rigidbody;
            Rigidbody.position = position;
        }
    }
}
