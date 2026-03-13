using System.Collections.Generic;
using Runtime.Core;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.UsersPanel
{
    public class UsersPanelPresenter : IPresenter
    {
        private readonly UsersPanelView _view;
        private readonly WorldViewDescription _viewDescription;
        private readonly UICoreModel _uiCoreModel;
        private readonly UIAudioService _audioService;

        private readonly Dictionary<string, VisualElement> _usersContainer = new();

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
            _uiCoreModel.PlayerSharedModel.Lobby.Members.OnRemoved += OnMemberRemoved;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged += OnHostChanged;
        }

        public void Disable()
        {
            _uiCoreModel.PlayerSharedModel.Lobby.Members.OnAdded -= OnMemberAdded;
            _uiCoreModel.PlayerSharedModel.Lobby.Members.OnRemoved -= OnMemberRemoved;
            _uiCoreModel.PlayerSharedModel.Lobby.OwnerId.OnChanged -= OnHostChanged;
            _view.Root.RemoveFromHierarchy();
            _view.UsersContainer.Clear();
            _usersContainer.Clear();
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
            _usersContainer.Add(username, user);

            OnHostChanged(_uiCoreModel.PlayerSharedModel.Lobby.OwnerId.Value);
        }

        private void OnHostChanged(string value)
        {
            foreach (var pair in _usersContainer)
            {
                if (pair.Key == value)
                {
                    var hostIcon = pair.Value.Q<VisualElement>("user-icon");
                    hostIcon.AddToClassList("host-icon-style");
                }
                else
                {
                    var hostIcon = pair.Value.Q<VisualElement>("user-icon");
                    hostIcon.RemoveFromClassList("host-icon-style");
                }
            }
        }

        private void OnMemberRemoved(string username)
        {
            OnHostChanged(_uiCoreModel.PlayerSharedModel.Lobby.OwnerId.Value);

            _view.UsersContainer.Remove(_usersContainer[username]);
            _usersContainer.Remove(username);
        }
    }
}
