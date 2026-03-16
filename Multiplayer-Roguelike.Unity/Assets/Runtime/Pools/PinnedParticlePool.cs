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
            if (parent == null || !parent.gameObject.activeInHierarchy)
            {
                return null;
            }

            var particle = GetItemFromPool();

            particle.transform.parent = parent;
            particle.transform.localPosition = Vector3.zero;
            particle.transform.localRotation = Quaternion.identity;

            AddItemToInProgress(particle);
            particle.OnComplete += ReturnToPool;
            particle.Enable();

            return particle;
        }

        protected override Particle CreateItem()
        {
            return Object.Instantiate(Prefab);
        }
    }
}
