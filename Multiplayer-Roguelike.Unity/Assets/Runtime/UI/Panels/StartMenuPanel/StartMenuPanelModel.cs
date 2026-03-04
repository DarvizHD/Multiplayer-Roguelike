using System;

namespace Runtime.UI.Panels.StartMenuPanel
{
    public class StartMenuPanelModel : IPanelModel
    {
        public string ViewId => "StartMenuPanel";
        public event Action OnSingleGameButtonClicked;
        public event Action OnCreateLobbyButtonClicked;
        public event Action OnJoinLobbyButtonClicked;
        public event Action OnExitButtonClicked;

        public void OnSingleGameButtonClickedInvoke()
        {
            OnSingleGameButtonClicked?.Invoke();
        }

        public void OnCreateLobbyButtonClickedInvoke()
        {
            OnCreateLobbyButtonClicked?.Invoke();
        }

        public void OnJoinLobbyButtonClickedInvoke()
        {
            OnJoinLobbyButtonClicked?.Invoke();
        }

        public void OnExitButtonClickedInvoke()
        {
            OnExitButtonClicked?.Invoke();
        }
    }
}
