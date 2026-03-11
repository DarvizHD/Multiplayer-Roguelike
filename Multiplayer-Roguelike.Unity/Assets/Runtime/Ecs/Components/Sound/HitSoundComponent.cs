using UnityEngine;

namespace Runtime.Ecs.Components.Sound
{
    public class HitSoundComponent : IComponent
    {
        public AudioClip Clip { get; }

        public HitSoundComponent(AudioClip clip)
        {
            Clip = clip;
        }
    }
}
