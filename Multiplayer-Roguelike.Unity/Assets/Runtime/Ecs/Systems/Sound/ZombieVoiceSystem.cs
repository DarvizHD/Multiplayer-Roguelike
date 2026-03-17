using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Sound
{
    public class ZombieVoiceSystem : BaseSystem
    {
        private QueryBuffer<ZombieVoiceComponent, AliveTagComponent> _aliveBuffer = new();
        private QueryBuffer<ZombieVoiceComponent, DeathEventComponent> _deathBuffer = new();

        protected override IQueryBuffer Buffer => _aliveBuffer;

        public override void Update(float deltaTime)
        {
            // TODO: SPLIT OT DIFFERENT SYSTEMS

            /*
            ComponentManager.Filter.Query(ref _deathBuffer);

            for (var i = 0; i < _deathBuffer.Count; i++)
            {
                var voice = _deathBuffer.Components1[i];
                voice.Source.Stop();
            }*/
        }

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
