using Runtime.Core;
using Runtime.UI.Menu.Panels.LobbyPanel;
using Runtime.UI.Menu.Panels.UsersPanel;
using Shared.Commands.Lobby;
using Shared.Commands.Session;

namespace Runtime.UI.Menu.Navigation.Screens
{
    public class LobbyScreen : IPresenter
    {
        private readonly Router _router;
        private readonly LobbyPanelPresenter _presenter;
        private readonly UsersPanelPresenter _usersPresenter;
        private readonly LobbyPanelModel _model;
        private readonly UICoreModel _uiCoreModel;
        private readonly UIAudioService _audioService;

        public LobbyScreen(Router router, LobbyPanelPresenter presenter,
            UsersPanelPresenter usersPresenter, LobbyPanelModel model, UICoreModel uiCoreModel, UIAudioService audioService)
        {
            _router = router;
            _presenter = presenter;
            _usersPresenter = usersPresenter;
            _model = model;
            _uiCoreModel = uiCoreModel;
            _audioService = audioService;
        }

        public void Enable()
        {
            _model.OnBackButtonClicked += HandleBack;
            _model.OnStartGameButtonClicked += HandleStartGame;
            _presenter.Enable();
            _usersPresenter.Enable();
        }

        public void Disable()
        {
            _model.OnBackButtonClicked -= HandleBack;
            _model.OnStartGameButtonClicked -= HandleStartGame;
            _presenter.Disable();
            _usersPresenter.Disable();
        }

        private void HandleBack()
        {
            var leaveCommand = new LeaveLobbyCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value, _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            leaveCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);

            _router.ToMainMenu();
        }

        private void HandleStartGame()
        {
            _audioService.PlayNavigate();

            var startGameCommand = new StartSessionCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value, _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            startGameCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }
    }
}
