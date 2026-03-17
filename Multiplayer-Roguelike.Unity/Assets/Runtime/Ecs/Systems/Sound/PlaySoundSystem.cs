using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Sound;
using Runtime.Sound.Constants;

namespace Runtime.Ecs.Systems.Sound
{
    public class PlaySoundSystem : BaseSystem
    {
        private readonly SoundModel _soundModel;
        private QueryBuffer<PlaySoundEventComponent, SfxContainerComponent> _buffer = new();
        protected override IQueryBuffer Buffer => _buffer;

        public PlaySoundSystem(SoundModel soundModel)
        {
            _soundModel = soundModel;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var soundEvent = _buffer.Components1[i];
            var sfx = _buffer.Components2[i];

            if (sfx.Container.activeInHierarchy)
            {
                _soundModel.Play(soundEvent.Clip, volume: SoundVolumeConstants.Sfx.Default, position: sfx.Container.transform.position);
            }

            ComponentManager.RemoveComponent<PlaySoundEventComponent>(entityId);
        }
    }
}
