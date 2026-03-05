using UnityEngine.UIElements;

namespace Runtime.UI.Panels.UsersPanel
{
    public class UsersPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; set; }
        public VisualElement UsersContainer { get; }

        public UsersPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("users-panel");
            ParentRoot = parentRoot;

            UsersContainer = Root.Q<VisualElement>("content");
        }
    }
}
