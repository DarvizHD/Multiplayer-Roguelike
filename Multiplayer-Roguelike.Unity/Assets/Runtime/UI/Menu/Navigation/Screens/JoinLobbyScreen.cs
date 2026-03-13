using Runtime.Core;
using Runtime.UI.Menu.Panels.JoinLobbyPanel;
using Shared.Commands.Lobby;

namespace Runtime.UI.Menu.Navigation.Screens
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
            _model.OnJoinButtonClicked += HandleTryJoin;
            _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.OnChanged += HandleJoin;

            _presenter.Enable();
        }

        public void Disable()
        {
            _model.OnBackButtonClicked -= HandleBack;
            _model.OnJoinButtonClicked -= HandleTryJoin;
            _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.OnChanged -= HandleJoin;

            _presenter.Disable();
        }

        private void HandleBack()
        {
            _router.GoBack();
        }

        private void HandleTryJoin()
        {
            var joinCommand = new JoinLobbyCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value, _model.LobbyCode);
            joinCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }

        private void HandleJoin(string value)
        {
            if (value == string.Empty)
            {
                return;
            }

            _router.NavigateTo(ScreenIds.Lobby);
        }
    }
}
