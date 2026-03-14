using System;

namespace Runtime.UI.HUD.SessionStatusPanel
{
    public class SessionStatusPanelModel
    {
        public event Action OnLeftSession;
        public event Action OnSessionRestarted;

        public void LeaveSession()
        {
            OnLeftSession?.Invoke();
        }

        public void RestartSession()
        {
            OnSessionRestarted?.Invoke();
        }
    }
}
