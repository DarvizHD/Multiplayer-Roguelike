using Runtime.UI.Menu;
using Runtime.UI.Menu.Runtime.UI.Navigation;
using Runtime.UI.Panels.JoinLobbyPanel;
using Shared.Commands;

namespace Runtime.UI.Navigation.Screens
{
    public class JoinLobbyScreen : IPresenter
    {
        private readonly Router _router;
        private readonly JoinLobbyPanelPresenter _presenter;
        private readonly JoinLobbyPanelModel _model;
        private readonly World _world;
        public JoinLobbyScreen(Router router, JoinLobbyPanelPresenter presenter, JoinLobbyPanelModel model, World world)
        {
            _router = router;
            _presenter = presenter;
            _model = model;
            _world = world;
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
            var joinCommand = new JoinLobbyCommand(_world.PlayerSharedModel.Nickname.Value, _model.LobbyCode);
            joinCommand.Write(_world.ServerConnectionModel.PlayerPeer);

            _router.NavigateTo(ScreenIds.HostLobby);
        }
    }
}
