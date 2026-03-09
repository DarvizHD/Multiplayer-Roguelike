using Runtime.Ecs.Components.Sound;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.Sound;
using UnityEngine;

namespace Runtime.Ecs.Systems.Sound
{
    public class PlaySoundSystem : BaseSystem
    {
        private readonly AudioSource _prefab = Resources.Load<AudioSource>(AudioResourcesConstants.AudioSource);
        private QueryBuffer<PlaySoundEventComponent, SfxContainerComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var soundEvent = _buffer.Components1[i];
                var sfx = _buffer.Components2[i];

                if (!sfx.Container.activeInHierarchy)
                {
                    ComponentManager.RemoveComponent<PlaySoundEventComponent>(entityId);
                    continue;
                }

                var source = Object.Instantiate(_prefab, sfx.Container.transform);
                source.clip = soundEvent.Clip;
                source.Play();

                Object.Destroy(source.gameObject, soundEvent.Clip.length);

                ComponentManager.RemoveComponent<PlaySoundEventComponent>(entityId);
            }
        }
    }
}
