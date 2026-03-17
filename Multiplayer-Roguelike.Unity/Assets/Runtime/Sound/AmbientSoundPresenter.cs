using Runtime.Core;
using Runtime.Sound.Constants;
using Shared.Models.GameSession;
using UnityEngine;

namespace Runtime.Sound
{
    public class AmbientSoundPresenter : IPresenter
    {
        private readonly SoundModel _soundModel;
        private readonly GameSessionSharedModel _gameSessionSharedModel;

        private readonly AudioClip _menuAmbient;
        private readonly AudioClip _gameplayAmbient;

        public AmbientSoundPresenter(SoundModel soundModel, GameSessionSharedModel gameSessionSharedModel)
        {
            _soundModel = soundModel;
            _gameSessionSharedModel = gameSessionSharedModel;

            _menuAmbient = Resources.Load<AudioClip>(AudioResourcesConstants.Ambient.MenuAmbientPath);
            _gameplayAmbient = Resources.Load<AudioClip>(AudioResourcesConstants.Ambient.GameplayAmbientPath);
        }

        public void Enable()
        {
            _gameSessionSharedModel.IsRun.OnChanged += HandleIsRunChanged;
            HandleIsRunChanged(_gameSessionSharedModel.IsRun.Value);
        }

        public void Disable()
        {
            _gameSessionSharedModel.IsRun.OnChanged -= HandleIsRunChanged;
            _soundModel.FadeOut(duration: 1f);
        }

        private void HandleIsRunChanged(bool isRun)
        {
            var current = isRun ? _menuAmbient : _gameplayAmbient;
            var next = isRun ? _gameplayAmbient : _menuAmbient;
            var volume = isRun ? SoundVolumeConstants.Ambient.Gameplay : SoundVolumeConstants.Ambient.Menu;

            _soundModel.FadeOut(duration: SoundVolumeConstants.Fade.Long, clipName: current.name);
            _soundModel.FadeIn(next, duration: SoundVolumeConstants.Fade.Long, volume: volume, loop: true);
        }
    }
}
