using UnityEngine;

namespace Runtime.Ecs.Components.Sound
{
    public class ZombieVoiceComponent : IComponent
    {
        public AudioSource Source { get; }
        public AudioClip Clip { get; }
        public float Delay { get; set; }

        public ZombieVoiceComponent(AudioSource source, AudioClip clip, float delay)
        {
            Source = source;
            Clip = clip;
            Delay = delay;
        }
    }
}
