using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.LoginPanel
{
    public class LoginPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; }
        public TextField UsernameTextField { get; }
        public TextField AddressTextField { get; }
        public VisualElement AddressContainer { get; }
        public Button ConfirmButton { get; }
        public Button OnlineButton { get; }

        public LoginPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("login-panel");
            ParentRoot = parentRoot;

            UsernameTextField = Root.Q<TextField>("username-text-field");
            AddressTextField = Root.Q<TextField>("address-text-field");
            ConfirmButton = Root.Q<Button>("confirm-button");
            OnlineButton = Root.Q<Button>("online-button");
            AddressContainer = Root.Q<VisualElement>("address-container");
        }
    }
}
