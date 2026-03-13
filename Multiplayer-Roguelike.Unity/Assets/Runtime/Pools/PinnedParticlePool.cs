using UnityEngine;

namespace Runtime.Pools
{
    public class PinnedParticlePool : Pool<Particle>
    {
        public PinnedParticlePool(Particle prefab) : base(prefab)
        {
        }

        public Particle Get(Transform parent)
        {
            var particle = Get();

            particle.transform.parent = parent;
            particle.transform.localPosition = Vector3.zero;
            particle.transform.localRotation = Quaternion.identity;

            return particle;
        }

        protected override Particle CreateItem()
        {
            return GameObject.Instantiate(Prefab);
        }
    }
}
