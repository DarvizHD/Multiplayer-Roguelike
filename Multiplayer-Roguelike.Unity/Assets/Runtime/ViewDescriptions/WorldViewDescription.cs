using Runtime.Pools;
using UnityEngine;

namespace Runtime.ViewDescriptions
{
    [CreateAssetMenu(fileName = "WorldViewDescription", menuName = "ViewDescriptions/WorldViewDescription")]
    public class WorldViewDescription : ScriptableObject
    {
        public UIViewDescription UI;

        public Particle DeathParticle;
        public Particle ShootParticle;
        public Particle DamageParticle;
    }
}
