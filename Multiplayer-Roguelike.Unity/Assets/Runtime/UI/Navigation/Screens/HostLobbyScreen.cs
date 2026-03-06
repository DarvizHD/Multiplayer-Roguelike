using Runtime.UI.Menu;
using Runtime.UI.Panels.HostLobbyPanel;
using Runtime.UI.Panels.UsersPanel;
using Shared.Commands;

namespace Runtime.UI.Navigation.Screens
{
    public class HostLobbyScreen : IPresenter
    {
        private readonly Router _router;
        private readonly HostLobbyPanelPresenter _hostPresenter;
        private readonly UsersPanelPresenter _usersPresenter;
        private readonly HostLobbyPanelModel _model;
        private readonly UICoreModel _uiCoreModel;

        public HostLobbyScreen(Router router, HostLobbyPanelPresenter hostPresenter,
            UsersPanelPresenter usersPresenter, HostLobbyPanelModel model, UICoreModel uiCoreModel)
        {
            _router = router;
            _hostPresenter = hostPresenter;
            _usersPresenter = usersPresenter;
            _model = model;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _model.OnBackButtonClicked += HandleBack;
            _model.OnStartGameButtonClicked += HandleStartGame;
            _hostPresenter.Enable();
            _usersPresenter.Enable();
        }

        public void Disable()
        {
            _model.OnBackButtonClicked -= HandleBack;
            _model.OnStartGameButtonClicked -= HandleStartGame;
            _hostPresenter.Disable();
            _usersPresenter.Disable();
        }

        private void HandleBack()
        {
            _router.GoBack();
        }

        private void HandleStartGame()
        {
            var startGameCommand = new StartSessionCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value,
                _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            startGameCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }
    }
}
