using UnityEngine;

namespace Runtime.Ecs.Components.Particles
{
    public class ShootParticlePointComponent : IComponent
    {
        public Transform Point;

        public ShootParticlePointComponent(Transform point)
        {
            Point = point;
        }
    }
}
