using UnityEngine.UIElements;

namespace Runtime.UI.HUD.SessionStatusPanel
{
    public class SessionStatusPanelView
    {
        public VisualElement Root;
        public Label StatusLabel;
        public Button RestartButton;
        public Button LobbyButton;

        public SessionStatusPanelView(UIHudView hudView)
        {
            Root = hudView.HudRoot.Q<VisualElement>("session-status-panel");

            StatusLabel = Root.Q<Label>("status-text");
            RestartButton = Root.Q<Button>("restart-button");
            LobbyButton = Root.Q<Button>("lobby-button");
        }
    }
}
