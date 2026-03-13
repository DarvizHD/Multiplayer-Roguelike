using Runtime.Core;
using UnityEngine;

namespace Runtime.UI
{
    public class DustParticlePresenter : IPresenter
    {
        private readonly Camera _camera;

        public DustParticlePresenter(Camera camera)
        {
            _camera = camera;
        }

        public void Enable()
        {
            _camera.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _camera.gameObject.SetActive(false);
        }
    }
}
