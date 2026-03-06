using UnityEngine.UIElements;

namespace Runtime.UI.Panels.StartMenuPanel
{
    public class StartMenuPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; set; }
        public Button SingleGameButton { get; set; }
        public Button CreateLobbyButton { get; set; }
        public Button JoinLobbyButton { get; set; }
        public Button ExitButton { get; set; }

        public StartMenuPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("menu-panel");
            ParentRoot = parentRoot;

            SingleGameButton = Root.Q<Button>("single-game-button");
            CreateLobbyButton = Root.Q<Button>("create-lobby-button");
            JoinLobbyButton = Root.Q<Button>("join-lobby-button");
            ExitButton = Root.Q<Button>("exit-button");
        }
    }
}
