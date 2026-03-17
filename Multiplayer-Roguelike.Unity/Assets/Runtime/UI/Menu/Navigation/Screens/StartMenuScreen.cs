using Runtime.Core;
using Runtime.UI.Menu.Panels.StartMenuPanel;
using Shared.Commands.Lobby;
using Shared.Commands.Player;
using Shared.Commands.Session;
using UnityEngine;

namespace Runtime.UI.Menu.Navigation.Screens
{
    public class StartMenuScreen : IPresenter
    {
        private readonly Router _router;
        private readonly StartMenuPanelPresenter _presenter;
        private readonly StartMenuPanelModel _model;
        private readonly UICoreModel _uiCoreModel;

        public StartMenuScreen(Router router, StartMenuPanelPresenter presenter, StartMenuPanelModel model, UICoreModel uiCoreModel)
        {
            _router = router;
            _presenter = presenter;
            _model = model;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _model.OnSingleGameButtonClicked += HandleSingleGame;
            _model.OnCreateLobbyButtonClicked += HandleCreateLobby;
            _model.OnJoinLobbyButtonClicked += HandleJoinLobby;
            _model.OnExitButtonClicked += HandleExit;
            _presenter.Enable();
        }

        public void Disable()
        {
            _model.OnSingleGameButtonClicked -= HandleSingleGame;
            _model.OnCreateLobbyButtonClicked -= HandleCreateLobby;
            _model.OnJoinLobbyButtonClicked -= HandleJoinLobby;
            _model.OnExitButtonClicked -= HandleExit;
            _presenter.Disable();
        }

        private void HandleSingleGame()
        {
            var createLobbyCommand = new CreateLobbyCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value);
            createLobbyCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);

            var startSessionCommand = new StartSessionCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value,
                _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            startSessionCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }

        private void HandleCreateLobby()
        {
            var createLobbyCommand = new CreateLobbyCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value);
            createLobbyCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);

            _router.NavigateTo(ScreenIds.Lobby);
        }

        private void HandleJoinLobby()
        {
            _router.NavigateTo(ScreenIds.JoinLobby);
        }

        private void HandleExit()
        {
            _uiCoreModel.ServerConnectionModel.PlayerDisconnect += HandleDisconnect;

            var logoutCommand = new LogoutCommand(_uiCoreModel.PlayerSharedModel.Nickname.Value);
            logoutCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }

        private void HandleDisconnect()
        {
            _uiCoreModel.ServerConnectionModel.PlayerDisconnect -= HandleDisconnect;
            Application.Quit();
        }
    }
}
