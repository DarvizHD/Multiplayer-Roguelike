using Runtime.Core;
using Runtime.UI.HUD;
using Runtime.UI.HUD.SessionStatusPanel;
using Runtime.UI.Menu.Navigation;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.UI.Menu
{
    public class UIPresenter : IPresenter
    {
        private readonly Router _router;
        private readonly UICoreModel _uiCoreModel;

        private readonly StartMenuPresenter _startMenuPresenter;
        private readonly UIHudPresenter _uiHudPresenter;

        private readonly SessionStatusPanelModel _sessionStatusPanelModel;
        private readonly SessionStatusPanelPresenter _sessionStatusPanelPresenter;

        public UIPresenter(Router router, UICoreModel uiCoreModel, WorldViewDescription worldViewDescription,
            UIDocument uiDocument, UIAudioService uiAudioService, UIHudView uiHudView)
        {
            _router = router;
            _uiCoreModel = uiCoreModel;

            _startMenuPresenter =
                new StartMenuPresenter(router, uiCoreModel, worldViewDescription, uiDocument, uiAudioService);
            _uiHudPresenter = new UIHudPresenter(uiHudView);

            var sessionStatusPanelView = new SessionStatusPanelView(uiHudView);
            _sessionStatusPanelModel = new SessionStatusPanelModel();
            _sessionStatusPanelPresenter = new SessionStatusPanelPresenter(sessionStatusPanelView, _sessionStatusPanelModel, _uiCoreModel);
        }

        public void Enable()
        {
            _uiCoreModel.GameSessionSharedModel.IsRun.OnChanged += HandleSessionRunChanged;
            _sessionStatusPanelModel.OnLeftSession += HandleLeftSession;

            _startMenuPresenter.Enable();
            _uiHudPresenter.Disable();
            _router.NavigateTo(ScreenIds.Login);
        }

        public void Disable()
        {
            _uiCoreModel.GameSessionSharedModel.IsRun.OnChanged -= HandleSessionRunChanged;
            _sessionStatusPanelModel.OnLeftSession -= HandleLeftSession;

            _startMenuPresenter.Disable();
        }

        private void HandleLeftSession()
        {
            _sessionStatusPanelPresenter.Disable();
            _uiHudPresenter.Disable();

            _startMenuPresenter.Enable();
            _router.NavigateTo(ScreenIds.Lobby);
        }

        private void HandleSessionRunChanged(bool isRun)
        {
            if (isRun)
            {
                _sessionStatusPanelPresenter.Disable();
                _startMenuPresenter.Disable();
                _uiHudPresenter.Enable();
            }
            else
            {
                _sessionStatusPanelPresenter.Enable();
            }
        }
    }
}
