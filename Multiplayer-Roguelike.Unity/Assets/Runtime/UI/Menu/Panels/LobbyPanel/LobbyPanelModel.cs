using System;

namespace Runtime.UI.Menu.Panels.LobbyPanel
{
    public class LobbyPanelModel : IPanelModel
    {
        public string ViewId => "LobbyPanel";

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
