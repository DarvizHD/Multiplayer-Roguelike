using Runtime.Sound.Constants;
using UnityEngine;

namespace Runtime.Ecs.Components.Sound
{
    public class ZombieVoiceComponent : IComponent
    {
        public float Delay;
        public readonly AudioSource Source;
        public readonly AudioClip Clip;
        public readonly float FadeDuration;
        public readonly float TargetVolume;

        public ZombieVoiceComponent(AudioSource source, AudioClip clip, float delay,
            float targetVolume = SoundVolumeConstants.Voice.Default,
            float fadeDuration = SoundVolumeConstants.Fade.Default)
        {
            Source = source;
            Clip = clip;
            Delay = delay;
            TargetVolume = targetVolume;
            FadeDuration = fadeDuration;
        }
    }
}
