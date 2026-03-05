using UnityEngine.UIElements;

namespace Runtime.UI.Panels.JoinLobbyPanel
{
    public class JoinLobbyPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; }
        public TextField LobbyCodeTextField { get; }
        public Button JoinButton { get; set; }
        public Button BackButton { get; set; }

        public JoinLobbyPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("join-lobby-panel");
            ParentRoot = parentRoot;

            LobbyCodeTextField = Root.Q<TextField>("lobby-code-text-field");
            JoinButton = Root.Q<Button>("join-button");
            BackButton = Root.Q<Button>("back-button");
        }
    }
}
