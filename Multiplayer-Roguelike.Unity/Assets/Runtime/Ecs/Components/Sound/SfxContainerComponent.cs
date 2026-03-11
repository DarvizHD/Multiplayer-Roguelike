using Runtime.ECS.Components;
using UnityEngine;

namespace Runtime.Ecs.Components.Sound
{
    public class SfxContainerComponent : IComponent
    {
        public GameObject Container { get; }

        public SfxContainerComponent(GameObject container)
        {
            Container = container;
        }
    }
}
