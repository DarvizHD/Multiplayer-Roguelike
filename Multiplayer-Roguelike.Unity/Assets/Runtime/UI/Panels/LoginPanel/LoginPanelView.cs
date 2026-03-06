using UnityEngine.UIElements;

namespace Runtime.UI.Panels.LoginPanel
{
    public class LoginPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; }
        public TextField UsernameTextField { get; }
        public Button ConfirmButton { get; set; }

        public LoginPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("login-panel");
            ParentRoot = parentRoot;

            UsernameTextField = Root.Q<TextField>("username-text-field");
            ConfirmButton = Root.Q<Button>("confirm-button");
        }
    }
}
