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
            Change();

            _gameSessionSharedModel.IsRun.OnChange += Change;
        }

        public void Disable()
        {
            _source.Stop();
            _gameSessionSharedModel.IsRun.OnChange -= Change;
        }

        private void Change()
        {
            _source.clip = Resources.Load<AudioClip>(_gameSessionSharedModel.IsRun.Value
                ? AudioResourcesConstants.Ambient.GameplayAmbientPath
                : AudioResourcesConstants.Ambient.MenuAmbientPath);

            _source.Play();
        }
    }
}
