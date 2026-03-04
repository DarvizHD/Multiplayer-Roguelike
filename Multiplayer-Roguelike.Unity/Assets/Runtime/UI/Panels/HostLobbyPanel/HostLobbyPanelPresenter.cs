namespace Runtime.UI.Panels.HostLobbyPanel
{
    public class HostLobbyPanelPresenter : IPresenter
    {
        private readonly HostLobbyPanelModel _model;
        private readonly HostLobbyPanelView _view;
        private readonly World _world;

        public HostLobbyPanelPresenter(HostLobbyPanelModel model, HostLobbyPanelView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);
            _view.BackButton.clicked += _model.OnBackButtonClickedInvoke;
            _view.StartGameButton.clicked += _model.OnStartGameButtonClickedInvoke;

            _world.PlayerSharedModel.Lobby.LobbyId.OnChange += HandleChangeLobbyCode;

            HandleChangeLobbyCode();
        }

        private void HandleChangeLobbyCode()
        {
            _view.LobbyCodeTextField.value = _world.PlayerSharedModel.Lobby.LobbyId.Value;
        }

        public void Disable()
        {
            _view.BackButton.clicked -= _model.OnBackButtonClickedInvoke;
            _view.StartGameButton.clicked -= _model.OnStartGameButtonClickedInvoke;
            _view.ParentRoot.Remove(_view.Root);
        }
    }
}
