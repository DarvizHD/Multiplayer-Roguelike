using Runtime.UI.Menu;
using Runtime.UI.Panels.JoinLobbyPanel;
using Shared.Commands.Lobby;

namespace Runtime.UI.Navigation.Screens
{
    public class JoinLobbyScreen : IPresenter
    {
        private readonly Router _router;
        private readonly JoinLobbyPanelPresenter _presenter;
        private readonly JoinLobbyPanelModel _model;
        private readonly UICoreModel _uiCoreModel;

        public JoinLobbyScreen(Router router, JoinLobbyPanelPresenter presenter, JoinLobbyPanelModel model, UICoreModel uiCoreModel)
        {
            _router = router;
            _presenter = presenter;
            _model = model;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _model.OnBackButtonClicked += HandleBack;
            _model.OnJoinButtonClicked += HandleJoin;
            _presenter.Enable();
        }

        public void Disable()
        {
            _model.OnBackButtonClicked -= HandleBack;
            _model.OnJoinButtonClicked -= HandleJoin;
            _presenter.Disable();
        }

        private void HandleBack()
        {
            _router.GoBack();
        }

        private void HandleJoin()
        {
            var joinCommand = new JoinLobbyCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value, _model.LobbyCode);
            joinCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);

            _router.NavigateTo(ScreenIds.HostLobby);
        }
    }
}
