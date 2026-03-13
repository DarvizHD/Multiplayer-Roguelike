using System.Collections.Generic;
using Runtime.Core;

namespace Runtime.UI.Menu.Navigation
{
    public class Router
    {
        private readonly Dictionary<string, IPresenter> _screens = new();
        private readonly Stack<string> _history = new();
        private readonly UIAudioService _audioService;

        public Router(UIAudioService audioService)
        {
            _audioService = audioService;
        }

        public void Register(string id, IPresenter screen)
        {
            _screens[id] = screen;
        }

        public void NavigateTo(string id)
        {
            NavigateInternal(id);

            _audioService.PlayNavigate();
        }

        public void NavigateSilent(string id)
        {
            NavigateInternal(id);
        }

        public void GoBack()
        {
            if (_history.Count <= 1)
            {
                return;
            }

            _screens[_history.Pop()].Disable();
            _screens[_history.Peek()].Enable();

            _audioService.PlayNavigate();
        }

        public void ToMainMenu()
        {
            foreach (var id in _history)
            {
                _screens[id].Disable();
            }
            _history.Clear();

            NavigateTo(ScreenIds.StartMenu);
        }

        public void Clear()
        {
            if (_history.TryPeek(out var current))
            {
                _screens[current].Disable();
            }

            _history.Clear();
        }

        private void NavigateInternal(string id)
        {
            if (_history.TryPeek(out var current))
            {
                _screens[current].Disable();
            }

            _history.Push(id);
            _screens[id].Enable();
        }
    }
}
