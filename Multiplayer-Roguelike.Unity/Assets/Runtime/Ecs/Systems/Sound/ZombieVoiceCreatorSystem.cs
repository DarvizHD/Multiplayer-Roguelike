using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Sound
{
    public class ZombieVoiceCreatorSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _aliveBuffer;
        private QueryBuffer<ZombieVoiceComponent, AliveTagComponent> _aliveBuffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _aliveBuffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var voice = _aliveBuffer.Components1[i];

            if (voice.Source.isPlaying)
            {
                return;
            }

            if (voice.Delay > 0f)
            {
                voice.Delay -= deltaTime;
                return;
            }

            voice.Source.clip = voice.Clip;
            voice.Source.Play();
            voice.Delay = Random.Range(0f, 2f);
        }
    }
}
