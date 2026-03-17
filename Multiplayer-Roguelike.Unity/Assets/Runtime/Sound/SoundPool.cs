using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Sound
{
    public class SoundPool
    {
        private readonly AudioSource _prefab;
        private readonly Transform _defaultRoot;
        private readonly Queue<AudioSource> _pool = new();

        public SoundPool(AudioSource prefab, Transform defaultRoot)
        {
            _prefab = prefab;
            _defaultRoot = defaultRoot;
        }

        public AudioSource Get(Transform root = null)
        {
            var parent = root ?? _defaultRoot;

            AudioSource source;
            if (_pool.Count > 0)
            {
                source = _pool.Dequeue();
                source.transform.SetParent(parent);
                source.transform.localPosition = Vector3.zero;
            }
            else
            {
                source = Object.Instantiate(_prefab, parent);
            }

            source.gameObject.SetActive(true);
            return source;
        }

        public void Return(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            source.transform.SetParent(_defaultRoot);
            _pool.Enqueue(source);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                Object.Destroy(_pool.Dequeue().gameObject);
            }
        }
    }
}
