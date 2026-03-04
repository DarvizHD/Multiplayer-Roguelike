using UnityEngine.UIElements;

namespace Runtime.UI.Panels.HostLobbyPanel
{
    public class HostLobbyPanelView
    {
        public VisualElement Root { get; }
        public VisualElement ParentRoot { get; }
        public TextField LobbyCodeTextField { get; }
        public Button StartGameButton { get; set; }
        public Button BackButton { get; set; }

        public HostLobbyPanelView(VisualTreeAsset asset, VisualElement parentRoot)
        {
            Root = asset.CloneTree().Q<VisualElement>("host-lobby-panel");
            ParentRoot = parentRoot;

            LobbyCodeTextField = Root.Q<TextField>("lobby-code-text-field");
            StartGameButton = Root.Q<Button>("start-game-button");
            BackButton = Root.Q<Button>("back-button");
        }
    }
}
