using Runtime.Core;
using  Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Sound;

namespace Runtime.Ecs.Systems.Sound
{
    public class ZombieVoiceCleanerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<ZombieVoiceComponent, DeathEventComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var voice = _buffer.Components1[i];

            if (!voice.Source.isPlaying)
            {
                return;
            }

            var monoBehavior = voice.Source.GetComponent<CoroutineRunner>() ?? voice.Source.gameObject.AddComponent<CoroutineRunner>();

            monoBehavior.StartCoroutine(AudioSourceFade.FadeOut(voice.Source, voice.FadeDuration));
        }
    }
}
