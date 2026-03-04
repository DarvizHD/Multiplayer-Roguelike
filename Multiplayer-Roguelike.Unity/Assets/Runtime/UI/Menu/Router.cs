using System.Collections.Generic;

namespace Runtime.UI.Menu
{
    namespace Runtime.UI.Navigation
    {
        public static class ScreenIds
        {
            public const string Login = "Login";
            public const string StartMenu = "StartMenu";
            public const string HostLobby = "HostLobby";
            public const string JoinLobby = "JoinLobby";
        }
    }

    public class Router
    {
        private readonly Dictionary<string, IPresenter> _screens = new();
        private readonly Stack<string> _history = new();

        public void Register(string id, IPresenter screen)
        {
            _screens[id] = screen;
        }

        public void NavigateTo(string id)
        {
            if (_history.TryPeek(out var current))
            {
                _screens[current].Disable();
            }

            _history.Push(id);
            _screens[id].Enable();
        }

        public void GoBack()
        {
            if (_history.Count <= 1)
            {
                return;
            }

            _screens[_history.Pop()].Disable();
            _screens[_history.Peek()].Enable();
        }

        public void Clear()
        {
            if (_history.TryPeek(out var current))
            {
                _screens[current].Disable();
            }

            _history.Clear();
        }
    }
}
