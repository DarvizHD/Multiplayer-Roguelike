using Runtime.Core;
using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels
{
    public abstract class BasePanelPresenter : IPresenter
    {
        protected readonly UIAudioService AudioService;
        protected abstract VisualElement Root { get; }

        protected BasePanelPresenter(UIAudioService audioService)
        {
            AudioService = audioService;
        }

        public virtual void Enable()
        {
            Root.Query<Button>().ForEach(b => b.clicked += AudioService.PlayNavigate);
        }

        public virtual void Disable()
        {
            Root.Query<Button>().ForEach(b => b.clicked -= AudioService.PlayNavigate);
        }
    }
}
