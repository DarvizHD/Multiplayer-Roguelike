using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Core;
using Runtime.Sound.Constants;
using UnityEngine;

namespace Runtime.Sound
{
    public class SoundPresenter : IPresenter
    {
        private readonly SoundModel _model;
        private readonly MonoBehaviour _runner;
        private readonly Transform _root;
        private readonly SoundPool _pool;

        private readonly Dictionary<string, AudioSource> _sources = new();

        private readonly Dictionary<string, Coroutine> _fades = new();

        public SoundPresenter(SoundModel model, MonoBehaviour runner, Transform root = null)
        {
            _model = model;
            _runner = runner;
            _root = root ?? runner.transform;
            _pool = new SoundPool(Resources.Load<AudioSource>(AudioResourcesConstants.AudioSource), _root);
        }

        public void Enable()
        {
            _model.OnPlay += OnPlay;
            _model.OnFadeIn += OnFadeIn;
            _model.OnFadeOut += OnFadeOut;
            _model.OnPause += OnPause;
            _model.OnResume += OnResume;
            _model.OnStop += OnStop;
        }

        public void Disable()
        {
            _model.OnPlay -= OnPlay;
            _model.OnFadeIn -= OnFadeIn;
            _model.OnFadeOut -= OnFadeOut;
            _model.OnPause -= OnPause;
            _model.OnResume -= OnResume;
            _model.OnStop -= OnStop;

            StopAllFades();

            foreach (var source in _sources.Values)
            {
                Object.Destroy(source.gameObject);
            }

            _sources.Clear();
        }

        private void OnPlay(SoundRequest request)
        {
            var source = GetOrCreateSource(request);
            StopFade(request.Clip.name);
            source.volume = request.Volume;
            source.loop = request.Loop;
            source.Play();
        }

        private void OnFadeIn(SoundRequest request, float duration)
        {
            var source = GetOrCreateSource(request);
            StopFade(request.Clip.name);
            source.volume = 0f;
            source.loop = request.Loop;
            source.Play();
            var coroutine = _runner.StartCoroutine(FadeRoutine(request.Clip.name, 0f, request.Volume, duration));
            _fades[request.Clip.name] = coroutine;
        }

        private void OnFadeOut(string clipName, float duration)
        {
            var targets = GetTargetSources(clipName);
            foreach (var (name, source) in targets)
            {
                StopFade(name);
                var coroutine =
                    _runner.StartCoroutine(FadeRoutine(name, source.volume, 0f, duration, stopOnComplete: true));
                _fades[name] = coroutine;
            }
        }

        private void OnPause(string clipName)
        {
            foreach (var (_, source) in GetTargetSources(clipName))
            {
                source.Pause();
            }
        }

        private void OnResume(string clipName)
        {
            foreach (var (_, source) in GetTargetSources(clipName))
            {
                source.UnPause();
            }
        }

        private void OnStop(string clipName)
        {
            foreach (var (name, source) in GetTargetSources(clipName))
            {
                StopFade(name);
                source.Stop();

                if (source.spatialBlend > 0f && source.transform.parent != _root)
                {
                    Object.Destroy(source.transform.parent.gameObject);
                }

                _pool.Return(source);
                _sources.Remove(name);
            }
        }

        private AudioSource GetOrCreateSource(SoundRequest request)
        {
            var key = request.Clip.name;

            if (_sources.TryGetValue(key, out var existing))
            {
                return existing;
            }

            Transform parent = null;
            if (request.Position.HasValue)
            {
                var anchor = new GameObject($"SoundAnchor_{key}")
                {
                    transform =
                    {
                        position = request.Position.Value
                    }
                };
                parent = anchor.transform;
            }

            var source = _pool.Get(parent);
            source.clip = request.Clip;
            source.spatialBlend = request.Position.HasValue ? 1f : 0f;
            _sources[key] = source;
            return source;
        }

        private IEnumerable<(string name, AudioSource source)> GetTargetSources(string clipName)
        {
            if (clipName == null)
            {
                foreach (var kvp in _sources)
                {
                    yield return (kvp.Key, kvp.Value);
                }
            }
            else if (_sources.TryGetValue(clipName, out var source))
            {
                yield return (clipName, source);
            }
        }

        private void StopFade(string clipName)
        {
            if (!_fades.TryGetValue(clipName, out var coroutine) || coroutine == null)
            {
                return;
            }

            _runner.StopCoroutine(coroutine);
            _fades.Remove(clipName);
        }

        private void StopAllFades()
        {
            foreach (var coroutine in _fades.Values.Where(coroutine => coroutine != null))
            {
                _runner.StopCoroutine(coroutine);
            }

            _fades.Clear();
        }

        private IEnumerator FadeRoutine(string clipName, float from, float to, float duration, bool stopOnComplete = false)
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                if (!_sources.TryGetValue(clipName, out var source))
                {
                    yield break;
                }

                elapsed += Time.deltaTime;
                source.volume = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            if (_sources.TryGetValue(clipName, out var s))
            {
                s.volume = to;

                if (stopOnComplete)
                {
                    s.Stop();
                }
            }

            _fades.Remove(clipName);
        }
    }
}
