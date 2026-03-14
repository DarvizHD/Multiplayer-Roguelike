using Runtime.Core;
using Shared.Commands.Session;
using UnityEngine.UIElements;

namespace Runtime.UI.HUD.SessionStatusPanel
{
    public class SessionStatusPanelPresenter : IPresenter
    {
        private readonly SessionStatusPanelView _view;
        private readonly SessionStatusPanelModel _model;
        private readonly UICoreModel _uiCoreModel;

        public SessionStatusPanelPresenter(SessionStatusPanelView view, SessionStatusPanelModel model,
            UICoreModel uiCoreModel)
        {
            _view = view;
            _model = model;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _view.Root.style.display = DisplayStyle.Flex;
            if (_uiCoreModel.PlayerSharedModel.Lobby.OwnerId.Value == _uiCoreModel.PlayerSharedModel.Nickname.Value)
            {
                _view.RestartButton.style.display = DisplayStyle.Flex;
                _view.RestartButton.clicked += OnRestartButtonClicked;
            }
            else
            {
                _view.RestartButton.style.display = DisplayStyle.None;
            }

            _view.LobbyButton.clicked += OnLobbyButtonClicked;
        }

        public void Disable()
        {
            _view.Root.style.display = DisplayStyle.None;

            _view.LobbyButton.clicked -= OnLobbyButtonClicked;
            _view.RestartButton.clicked -= OnRestartButtonClicked;
        }

        private void OnLobbyButtonClicked()
        {
            var leaveCommand = new LeaveSessionCommand
            (
                _uiCoreModel.PlayerSharedModel.Nickname.Value,
                _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value
            );
            leaveCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);

            _model.LeaveSession();
        }

        private void OnRestartButtonClicked()
        {
            var startGameCommand = new StartSessionCommand
            (
                _uiCoreModel.PlayerSharedModel.Nickname.Value,
                _uiCoreModel.PlayerSharedModel.Lobby.LobbyId.Value
            );
            startGameCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }
    }
}
