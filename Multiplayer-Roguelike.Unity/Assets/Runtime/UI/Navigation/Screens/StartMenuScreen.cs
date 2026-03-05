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
        private readonly World _world;
        public StartMenuScreen(Router router, StartMenuPanelPresenter presenter, StartMenuPanelModel model, World world)
        {
            _router = router;
            _presenter = presenter;
            _model = model;
            _world = world;
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
            var createLobbyCommand = new CreateLobbyCommand(_world.PlayerSharedModel.Nickname.Value);
            createLobbyCommand.Write(_world.ServerConnectionModel.PlayerPeer);

            var startSessionCommand = new StartSessionCommand(_world.PlayerSharedModel.Nickname.Value, _world.PlayerSharedModel.Lobby.LobbyId.Value);
            startSessionCommand.Write(_world.ServerConnectionModel.PlayerPeer);
        }

        private void HandleCreateLobby()
        {
            var createLobbyCommand = new CreateLobbyCommand(_world.PlayerSharedModel.Nickname.Value);
            createLobbyCommand.Write(_world.ServerConnectionModel.PlayerPeer);

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
