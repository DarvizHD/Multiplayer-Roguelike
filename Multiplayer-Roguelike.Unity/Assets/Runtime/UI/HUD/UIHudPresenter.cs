using Runtime.Core;
using UnityEngine.UIElements;

namespace Runtime.UI.HUD
{
    public class UIHudPresenter : IPresenter
    {
        private readonly UIHudView _view;

        public UIHudPresenter(UIHudView view)
        {
            _view = view;
        }

        public void Enable()
        {
            _view.HudRoot.style.display = DisplayStyle.Flex;
        }

        public void Disable()
        {
            _view.HudRoot.style.display = DisplayStyle.None;
        }
    }
}
