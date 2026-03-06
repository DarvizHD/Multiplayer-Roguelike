using System;

namespace Runtime.UI.Panels.LoginPanel
{
    public class LoginPanelModel : IPanelModel
    {
        public event Action OnConfirmed;
        public string ViewId => "LoginPanel";
        public string Username { get; private set; }

        public void SetUsername(string username)
        {
            Username = username;
        }

        public void Confirm()
        {
            OnConfirmed?.Invoke();
        }
    }
}
