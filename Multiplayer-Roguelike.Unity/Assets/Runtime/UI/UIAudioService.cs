using UnityEngine;

namespace Runtime.UI
{
    public class UIAudioService
    {
        private readonly AudioSource _audioSource;
        private readonly AudioClip _navigateClip;
        private readonly AudioClip _joinToLobbyClip;

        public UIAudioService(AudioSource audioSource, AudioClip navigateClip, AudioClip joinToLobbyClip)
        {
            _audioSource = audioSource;
            _navigateClip = navigateClip;
            _joinToLobbyClip = joinToLobbyClip;
        }

        public void PlayNavigate()
        {
            _audioSource.PlayOneShot(_navigateClip);
        }

        public void PlayJoinToLobby()
        {
            _audioSource.PlayOneShot(_joinToLobbyClip);
        }
    }
}
