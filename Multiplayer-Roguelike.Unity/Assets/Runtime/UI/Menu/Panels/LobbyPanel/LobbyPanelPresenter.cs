using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.LobbyPanel
{
    public class LobbyPanelPresenter : BasePanelPresenter
    {
        protected override VisualElement Root => _view.Root;

        private readonly LobbyPanelModel _model;
        private readonly LobbyPanelView _view;
        private readonly UICoreModel _uiCoreModel;

        public LobbyPanelPresenter(LobbyPanelModel model, LobbyPanelView view,
            UICoreModel uiCoreModel, UIAudioService audioService) : base(audioService)
        {
            _model = model;
            _view = view;
            _uiCoreModel = uiCoreModel;
        }

        public override void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            base.Enable();

            _view.BackButton.clicked += _model.OnBackButtonClickedInvoke;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged += HandleChangeOwner;
            _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.OnChanged += HandleChangeLobbyCode;

            HandleChangeLobbyCode(_uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            HandleChangeOwner(_uiCoreModel.PlayerSharedModel.Lobby.OwnerId.Value);
        }

        public override void Disable()
        {
            _view.BackButton.clicked -= _model.OnBackButtonClickedInvoke;
            _view.StartGameButton.clicked -= _model.OnStartGameButtonClickedInvoke;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged -= HandleChangeOwner;
            _view.Root.RemoveFromHierarchy();
            base.Disable();
        }

        private void HandleChangeOwner(string value)
        {
            if (value == _uiCoreModel.PlayerSharedModel.Nickname.Value)
            {
                _view.StartGameButton.style.display = DisplayStyle.Flex;
                _view.StartGameButton.clicked += _model.OnStartGameButtonClickedInvoke;
            }
            else
            {
                _view.StartGameButton.style.display = DisplayStyle.None;
            }
        }

        private void HandleChangeLobbyCode(string value)
        {
            _view.LobbyCodeTextField.value = value;
        }
    }
}
