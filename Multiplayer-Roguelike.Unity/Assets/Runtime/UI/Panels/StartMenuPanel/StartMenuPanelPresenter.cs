namespace Runtime.UI.Panels.StartMenuPanel
{
    public class StartMenuPanelPresenter : IPresenter
    {
        private readonly StartMenuPanelModel _model;
        private readonly StartMenuPanelView _view;

        public StartMenuPanelPresenter(StartMenuPanelModel model, StartMenuPanelView view)
        {
            _model = model;
            _view = view;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            _view.SingleGameButton.clicked += _model.OnSingleGameButtonClickedInvoke;
            _view.CreateLobbyButton.clicked += _model.OnCreateLobbyButtonClickedInvoke;
            _view.JoinLobbyButton.clicked += _model.OnJoinLobbyButtonClickedInvoke;
            _view.ExitButton.clicked += _model.OnExitButtonClickedInvoke;
        }

        public void Disable()
        {
            _view.SingleGameButton.clicked -= _model.OnSingleGameButtonClickedInvoke;
            _view.CreateLobbyButton.clicked -= _model.OnCreateLobbyButtonClickedInvoke;
            _view.JoinLobbyButton.clicked -= _model.OnJoinLobbyButtonClickedInvoke;
            _view.ExitButton.clicked -= _model.OnExitButtonClickedInvoke;

            _view.ParentRoot.Remove(_view.Root);
        }
    }
}
