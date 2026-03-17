using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Sound;
using Runtime.Sound.Constants;
using UnityEngine;

namespace Runtime.Ecs.Systems.Sound
{
    public class PlaySoundSystem : BaseSystem
    {
        private readonly AudioSource _prefab = Resources.Load<AudioSource>(AudioResourcesConstants.AudioSource);
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PlaySoundEventComponent, SfxContainerComponent> _buffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var soundEvent = _buffer.Components1[i];
            var sfx = _buffer.Components2[i];

            if (!sfx.Container.activeInHierarchy)
            {
                ComponentManager.RemoveComponent<PlaySoundEventComponent>(entityId);
                return;
            }

            var source = Object.Instantiate(_prefab, sfx.Container.transform);
            source.clip = soundEvent.Clip;
            source.Play();

            Object.Destroy(source.gameObject, soundEvent.Clip.length);

            ComponentManager.RemoveComponent<PlaySoundEventComponent>(entityId);
        }
    }
}
