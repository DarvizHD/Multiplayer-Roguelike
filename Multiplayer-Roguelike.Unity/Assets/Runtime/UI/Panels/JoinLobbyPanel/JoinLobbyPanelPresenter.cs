namespace Runtime.UI.Panels.JoinLobbyPanel
{
    public class JoinLobbyPanelPresenter : IPresenter
    {
        private readonly JoinLobbyPanelModel _model;
        private readonly JoinLobbyPanelView _view;
        private readonly UICoreModel _uiCoreModel;

        public JoinLobbyPanelPresenter(JoinLobbyPanelModel model, JoinLobbyPanelView view, UICoreModel uiCoreModel)
        {
            _model = model;
            _view = view;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);
            _view.BackButton.clicked += _model.OnBackButtonClickedInvoke;
            _view.JoinButton.clicked += HandleJoinButtonClick;
        }

        private void HandleJoinButtonClick()
        {
            _model.LobbyCode = _view.LobbyCodeTextField.value;
            _model.OnJoinButtonClickedInvoke();
        }

        public void Disable()
        {
            _view.BackButton.clicked -= _model.OnBackButtonClickedInvoke;
            _view.JoinButton.clicked -= HandleJoinButtonClick;
            _view.ParentRoot.Remove(_view.Root);
        }
    }
}
