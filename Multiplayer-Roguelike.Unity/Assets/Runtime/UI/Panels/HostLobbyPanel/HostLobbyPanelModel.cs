using System;

namespace Runtime.UI.Panels.HostLobbyPanel
{
    public class HostLobbyPanelModel : IPanelModel
    {
        public string ViewId => "HostLobbyPanel";

        public string LobbyId;

        public event Action OnBackButtonClicked;
        public event Action OnStartGameButtonClicked;

        public void OnBackButtonClickedInvoke()
        {
            OnBackButtonClicked?.Invoke();
        }

        public void OnStartGameButtonClickedInvoke()
        {
            OnStartGameButtonClicked?.Invoke();
        }
    }
}
