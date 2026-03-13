using UnityEngine;

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

        return particle;
    }

    protected override Particle CreateItem()
    {
        return GameObject.Instantiate(Prefab);
    }
}
