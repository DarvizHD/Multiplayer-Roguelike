using UnityEngine.UIElements;

namespace Runtime.UI.Panels.LobbyPanel
{
    public class LobbyPanelPresenter : IPresenter
    {
        private readonly LobbyPanelModel _model;
        private readonly LobbyPanelView _view;
        private readonly UICoreModel _uiCoreModel;

        public LobbyPanelPresenter(LobbyPanelModel model, LobbyPanelView view, UICoreModel uiCoreModel)
        {
            _model = model;
            _view = view;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);
            _view.BackButton.clicked += _model.OnBackButtonClickedInvoke;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged += HandleChangeOwner;
            _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.OnChanged += HandleChangeLobbyCode;

            HandleChangeLobbyCode(_uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value);
            HandleChangeOwner(_uiCoreModel.PlayerSharedModel.Lobby.OwnerId.Value);
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

        public void Disable()
        {
            _view.BackButton.clicked -= _model.OnBackButtonClickedInvoke;
            _view.StartGameButton.clicked -= _model.OnStartGameButtonClickedInvoke;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged -= HandleChangeOwner;
            _view.Root.RemoveFromHierarchy();
        }
    }
}
