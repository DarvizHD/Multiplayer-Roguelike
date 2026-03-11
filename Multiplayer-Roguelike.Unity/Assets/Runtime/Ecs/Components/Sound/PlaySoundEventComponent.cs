using Runtime.ECS.Components;
using UnityEngine;

namespace Runtime.Ecs.Components.Sound
{
    public class PlaySoundEventComponent : IComponent
    {
        public AudioClip Clip { get; }

        public PlaySoundEventComponent(AudioClip clip)
        {
            Clip = clip;
        }
    }
}
