using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.Parallax
{
    public class ParallaxPresenter : IPresenter
    {
        private const float _overflow = 1f;
        private const float _speed = 20f;

        private readonly ParallaxView _view;

        public ParallaxPresenter(ParallaxView view)
        {
            _view = view;
        }

        public void Enable()
        {
            _view.Root.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        public void Disable()
        {
            _view.Root.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            var worldCenter = _view.Root.worldBound.center;

            var offset = new Vector2(evt.mousePosition.x - worldCenter.x, evt.mousePosition.y - worldCenter.y);

            var normalized = new Vector2(
                Mathf.Clamp(offset.x / (worldCenter.x), -_overflow, _overflow),
                Mathf.Clamp(offset.y / (worldCenter.y), -_overflow, _overflow)
            );

            _view.Background.style.translate = new Translate(new Length(normalized.x * -_speed), new Length(normalized.y * -_speed));
        }
    }
}
