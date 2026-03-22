using System.Collections;
using UnityEngine;

namespace Runtime.Sound
{
    public static class AudioSourceFade
    {
        public static IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
            {
                source.volume = 0f;
                var elapsed = 0f;

                while (elapsed < duration && source)
                {
                    elapsed += Time.deltaTime;
                    source.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
                    yield return null;
                }

                if (source)
                {
                    source.volume = targetVolume;
                }
            }

            public static IEnumerator FadeOut(AudioSource source, float duration)
            {
                var startVolume = source.volume;
                var elapsed = 0f;

                while (elapsed < duration && source)
                {
                    elapsed += Time.deltaTime;
                    source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                    yield return null;
                }

                if (source)
                {
                    source.Stop();
                }
            }
    }
}
