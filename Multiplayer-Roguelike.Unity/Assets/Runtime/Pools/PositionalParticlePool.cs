using UnityEngine;

namespace Runtime.Pools
{
    public class PositionalParticlePool : Pool<Particle>
    {
        private readonly Transform _parent;

        public PositionalParticlePool(Particle prefab) : base(prefab)
        {
            _parent = new GameObject($"{prefab.name}_Pool").transform;
        }

        public Particle Get(Vector3 position)
        {
            var particle = Get();

            particle.transform.position = position;

            return particle;
        }

        protected override Particle CreateItem()
        {
            return Object.Instantiate(Prefab, _parent);
        }
    }
}
