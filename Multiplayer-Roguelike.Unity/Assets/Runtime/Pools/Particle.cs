using System;
using UnityEngine;

public class Particle : MonoBehaviour, IPoolItem
{
    public event Action<IPoolItem> OnComplete;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private GameObject _gameObject;

    public void Enable()
    {
        _particleSystem.Play();
        _gameObject.SetActive(true);

        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void Disable()
    {
        _particleSystem.Stop();
        _gameObject.SetActive(false);
    }

    private void OnParticleSystemStopped()
    {
        OnComplete?.Invoke(this);
        _gameObject.SetActive(false);
    }
}