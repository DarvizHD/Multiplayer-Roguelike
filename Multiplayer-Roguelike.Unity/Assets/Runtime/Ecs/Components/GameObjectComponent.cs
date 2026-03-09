using UnityEngine;

namespace Runtime.ECS.Components
{
    public class GameObjectComponent : IComponent
    {
        public GameObject GameObject { get; set; }

        public GameObjectComponent(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}
