using UnityEngine.UIElements;

namespace Runtime.UI.Panels.UsersPanel
{
    public class UsersPanelPresenter : IPresenter
    {
        private readonly UsersPanelView _view;
        private readonly WorldViewDescription _viewDescription;
        private readonly UICoreModel _uiCoreModel;
        private readonly UIAudioService _audioService;

        public UsersPanelPresenter(UsersPanelView view, WorldViewDescription viewDescription, UICoreModel uiCoreModel, UIAudioService audioService)
        {
            _view = view;
            _viewDescription = viewDescription;
            _uiCoreModel = uiCoreModel;
            _audioService = audioService;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            _uiCoreModel.PlayerSharedModel.Lobby.Members.OnAdded += OnMemberAdded;

            foreach (var username in _uiCoreModel.PlayerSharedModel.Lobby.Members.Values)
            {
                OnMemberAdded(username);
            }
        }

        private void OnMemberAdded(string username)
        {
            if (_uiCoreModel.PlayerSharedModel.Lobby.Members.Values.Count > 1)
            {
                _audioService.PlayJoinToLobby();
            }

            var userAsset = _viewDescription.UI.UserAsset;
            var user = userAsset.CloneTree().Q<VisualElement>("user-panel");
            user.Q<Label>("username-text").text = username;
            _view.UsersContainer.Add(user);
        }

        public void Disable()
        {
            _view.ParentRoot.Remove(_view.Root);
            _view.UsersContainer.Clear();
        }
    }
}
