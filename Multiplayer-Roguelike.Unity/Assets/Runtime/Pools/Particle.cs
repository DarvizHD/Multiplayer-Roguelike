using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Pools
{
    public class Particle : MonoBehaviour, IPoolItem
    {
        public event Action<IPoolItem> OnComplete;

        [SerializeField] private List<ParticleSystem> _particleSystems;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private float _duration = 0.5f;

        private Coroutine _coroutine;

        public void Enable()
        {
            _gameObject.SetActive(true);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            if (_gameObject != null)
            {
                _gameObject.SetActive(true);
            }

            if (_particleSystems != null)
            {
                foreach (var ps in _particleSystems)
                {
                    if (ps != null)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        ps.Play();
                    }
                }
            }

            _coroutine = StartCoroutine(DisableAfterDelay());
        }

        public void Disable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _gameObject.SetActive(false);
        }

        private IEnumerator DisableAfterDelay()
        {
            yield return new WaitForSeconds(_duration);

            foreach (var ps in _particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            _gameObject.SetActive(false);
            _coroutine = null;
            OnComplete?.Invoke(this);
        }
    }
}
