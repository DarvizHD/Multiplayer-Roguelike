using Runtime.UI.Menu;
using Runtime.UI.Panels.StartMenuPanel;
using Shared.Commands;
using UnityEngine;

namespace Runtime.UI.Navigation.Screens
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

            _router.NavigateTo(ScreenIds.HostLobby);
        }

        private void HandleJoinLobby()
        {
            _router.NavigateTo(ScreenIds.JoinLobby);
        }

        private void HandleExit()
        {
            Application.Quit();
        }
    }
}
