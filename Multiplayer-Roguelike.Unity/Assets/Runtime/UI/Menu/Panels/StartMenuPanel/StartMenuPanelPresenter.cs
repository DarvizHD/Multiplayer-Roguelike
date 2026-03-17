using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.StartMenuPanel
{
    public class StartMenuPanelPresenter : BasePanelPresenter
    {
        protected override VisualElement Root => _view.Root;

        private readonly StartMenuPanelModel _model;
        private readonly StartMenuPanelView _view;

        public StartMenuPanelPresenter(StartMenuPanelModel model, StartMenuPanelView view, UIAudioService audioService) : base(audioService)
        {
            _model = model;
            _view = view;
        }

        public override void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            base.Enable();

            _view.SingleGameButton.clicked += _model.OnSingleGameButtonClickedInvoke;
            _view.CreateLobbyButton.clicked += _model.OnCreateLobbyButtonClickedInvoke;
            _view.JoinLobbyButton.clicked += _model.OnJoinLobbyButtonClickedInvoke;
            _view.ExitButton.clicked += _model.OnExitButtonClickedInvoke;
        }

        public override void Disable()
        {
            _view.SingleGameButton.clicked -= _model.OnSingleGameButtonClickedInvoke;
            _view.CreateLobbyButton.clicked -= _model.OnCreateLobbyButtonClickedInvoke;
            _view.JoinLobbyButton.clicked -= _model.OnJoinLobbyButtonClickedInvoke;
            _view.ExitButton.clicked -= _model.OnExitButtonClickedInvoke;

            base.Disable();

            _view.Root.RemoveFromHierarchy();
        }
    }
}
