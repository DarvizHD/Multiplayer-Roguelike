using Shared.Models.GameSession;
using UnityEngine;

namespace Runtime.Sound
{
    public class AmbientSoundPresenter : IPresenter
    {
        private readonly AudioSource _source;
        private readonly GameSessionSharedModel _gameSessionSharedModel;

        public AmbientSoundPresenter(AudioSource source, GameSessionSharedModel gameSessionSharedModel)
        {
            _source = source;
            _gameSessionSharedModel = gameSessionSharedModel;
        }

        public void Enable()
        {
            HandelIsRunChanged(_gameSessionSharedModel.IsRun.Value);

            _gameSessionSharedModel.IsRun.OnChanged += HandelIsRunChanged;
        }

        public void Disable()
        {
            _source.Stop();
            _gameSessionSharedModel.IsRun.OnChanged -= HandelIsRunChanged;
        }

        private void HandelIsRunChanged(bool value)
        {
            _source.clip = Resources.Load<AudioClip>(value
                ? AudioResourcesConstants.Ambient.GameplayAmbientPath
                : AudioResourcesConstants.Ambient.MenuAmbientPath);

            _source.Play();
        }
    }
}
