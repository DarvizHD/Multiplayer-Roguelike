using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.JoinLobbyPanel
{
    public class JoinLobbyPanelPresenter : BasePanelPresenter
    {
        protected override VisualElement Root => _view.Root;

        private readonly JoinLobbyPanelModel _model;
        private readonly JoinLobbyPanelView _view;

        public JoinLobbyPanelPresenter(JoinLobbyPanelModel model, JoinLobbyPanelView view, UIAudioService audioService) : base(audioService)
        {
            _model = model;
            _view = view;
        }

        public override void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            base.Enable();

            _view.BackButton.clicked += _model.OnBackButtonClickedInvoke;
            _view.JoinButton.clicked += HandleJoinButtonClick;
        }

        public override void Disable()
        {
            _view.BackButton.clicked -= _model.OnBackButtonClickedInvoke;
            _view.JoinButton.clicked -= HandleJoinButtonClick;
            _view.Root.RemoveFromHierarchy();

            base.Disable();
        }

        private void HandleJoinButtonClick()
        {
            _model.LobbyCode = _view.LobbyCodeTextField.value;
            _model.OnJoinButtonClickedInvoke();
        }
    }
}
