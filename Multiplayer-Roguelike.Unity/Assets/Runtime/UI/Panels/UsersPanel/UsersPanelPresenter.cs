using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.Panels.UsersPanel
{
    public class UsersPanelPresenter : IPresenter
    {
        private readonly UsersPanelView _view;
        private readonly WorldViewDescription _viewDescription;
        private readonly World _world;
        public UsersPanelPresenter(UsersPanelView view, WorldViewDescription viewDescription, World world)
        {
            _view = view;
            _viewDescription = viewDescription;
            _world = world;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            _world.PlayerSharedModel.Lobby.Members.OnAdded += OnMemberAdded;

            foreach (var username in _world.PlayerSharedModel.Lobby.Members.Values)
            {
                OnMemberAdded(username);
            }
        }

        private void OnMemberAdded(string username)
        {
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
