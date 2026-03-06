using Runtime.ServerInteraction;
using Runtime.UI.Panels.HostLobbyPanel;
using Runtime.UI.Panels.JoinLobbyPanel;
using Runtime.UI.Panels.LoginPanel;
using Runtime.UI.Panels.StartMenuPanel;
using Runtime.UI.Panels.UsersPanel;
using Shared.Models;

namespace Runtime.UI
{
    public class UICoreModel
    {
        public LoginPanelModel LoginPanelModel { get; } = new();
        public StartMenuPanelModel StartMenuPanelModel { get; } = new();
        public HostLobbyPanelModel HostLobbyPanelModel { get; } = new();
        public JoinLobbyPanelModel JoinLobbyPanelModel { get; } = new();
        public UsersPanelModel UsersPanelModel { get; } = new();
        public PlayerSharedModel PlayerSharedModel { get; private set; }
        public ServerConnectionModel ServerConnectionModel { get; private set; }
        public GameSessionSharedModel GameSessionSharedModel { get; private set; }

        public void Setup(PlayerSharedModel playerSharedModel, ServerConnectionModel serverConnectionModel,
            GameSessionSharedModel gameSessionSharedModel)
        {
            PlayerSharedModel = playerSharedModel;
            ServerConnectionModel = serverConnectionModel;
            GameSessionSharedModel = gameSessionSharedModel;
        }
    }
}
