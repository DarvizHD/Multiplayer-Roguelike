using System;

namespace Runtime.UI.Panels.JoinLobbyPanel
{
    public class JoinLobbyPanelModel : IPanelModel
    {
        public string ViewId => "JoinLobbyPanel";

        public string LobbyCode;
        public event Action OnBackButtonClicked;
        public event Action OnJoinButtonClicked;

        public void OnBackButtonClickedInvoke()
        {
            OnBackButtonClicked?.Invoke();
        }

        public void OnJoinButtonClickedInvoke()
        {
            OnJoinButtonClicked?.Invoke();
        }
    }
}
