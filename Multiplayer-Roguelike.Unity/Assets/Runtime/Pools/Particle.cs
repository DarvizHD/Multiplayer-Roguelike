using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Pools
{
    public class Particle : MonoBehaviour, IPoolItem
    {
        public event Action<IPoolItem> OnComplete;

        [SerializeField] private List<ParticleSystem> _particleSystems;

        [SerializeField] private GameObject _gameObject;

        public void Enable()
        {
            _gameObject.SetActive(true);

            foreach (var ps in _particleSystems)
            {
                var main = ps.main;
                main.stopAction = ParticleSystemStopAction.Callback;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Play();
            }
        }

        public void Disable()
        {
            _gameObject.SetActive(false);
        }

        private void OnParticleSystemStopped()
        {
            _gameObject.SetActive(false);
            OnComplete?.Invoke(this);
        }
    }
}
