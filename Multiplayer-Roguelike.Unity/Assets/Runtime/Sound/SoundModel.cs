using System;
using UnityEngine;

namespace Runtime.Sound
{
    public class SoundModel
    {
        public event Action<SoundRequest> OnPlay;
        public event Action<SoundRequest, float> OnFadeIn;
        public event Action<string, float> OnFadeOut;
        public event Action<string> OnPause;
        public event Action<string> OnResume;
        public event Action<string> OnStop;

        public void Play(AudioClip clip, float volume = 1f, bool loop = false, Vector3? position = null)
        {
            OnPlay?.Invoke(new SoundRequest { Clip = clip, Volume = volume, Loop = loop, Position = position });
        }

        public void FadeIn(AudioClip clip, float duration, float volume = 1f, bool loop = false, Vector3? position = null)
        {
            OnFadeIn?.Invoke(new SoundRequest { Clip = clip, Volume = volume, Loop = loop, Position = position }, duration);
        }

        public void FadeOut(float duration, string clipName = null)
        {
            OnFadeOut?.Invoke(clipName, duration);
        }

        public void Pause(string clipName = null)
        {
            OnPause?.Invoke(clipName);
        }

        public void Resume(string clipName = null)
        {
            OnResume?.Invoke(clipName);
        }

        public void Stop(string clipName = null)
        {
            OnStop?.Invoke(clipName);
        }
    }
}
