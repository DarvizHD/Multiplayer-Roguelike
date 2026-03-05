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
        private readonly World _world;
        public HostLobbyScreen(Router router, HostLobbyPanelPresenter hostPresenter,
            UsersPanelPresenter usersPresenter, HostLobbyPanelModel model, World world)
        {
            _router = router;
            _hostPresenter = hostPresenter;
            _usersPresenter = usersPresenter;
            _model = model;
            _world = world;
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
            var startGameCommand = new StartSessionCommand(_world.PlayerSharedModel.Nickname.Value, _world.PlayerSharedModel.Lobby.LobbyId.Value);
            startGameCommand.Write(_world.ServerConnectionModel.PlayerPeer);
        }
    }
}
