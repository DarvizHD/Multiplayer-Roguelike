using Runtime.ServerInteraction;
using Runtime.UI.Navigation;
using Runtime.UI.Panels.HostLobbyPanel;
using Runtime.UI.Panels.JoinLobbyPanel;
using Runtime.UI.Panels.LoginPanel;
using Runtime.UI.Panels.StartMenuPanel;
using Runtime.UI.Panels.UsersPanel;
using Shared.Models;

namespace Runtime.UI
{
    public class World
    {
        public LoginPanelModel LoginPanelModel { get; }
        public StartMenuPanelModel StartMenuPanelModel { get; }
        public HostLobbyPanelModel HostLobbyPanelModel { get; }
        public JoinLobbyPanelModel JoinLobbyPanelModel { get; }
        public UsersPanelModel UsersPanelModel { get; }
        public PlayerSharedModel PlayerSharedModel { get; private set; }
        public ServerConnectionModel ServerConnectionModel { get; private set; }
        public GameSessionSharedModel GameSessionSharedModel { get; private set; }

        public World()
        {
            LoginPanelModel = new LoginPanelModel();
            StartMenuPanelModel = new StartMenuPanelModel();
            HostLobbyPanelModel = new HostLobbyPanelModel();
            JoinLobbyPanelModel = new JoinLobbyPanelModel();
            UsersPanelModel = new UsersPanelModel();
        }

        public void Setup(PlayerSharedModel playerSharedModel, ServerConnectionModel serverConnectionModel, GameSessionSharedModel gameSessionSharedModel)
        {
            PlayerSharedModel = playerSharedModel;
            ServerConnectionModel = serverConnectionModel;
            GameSessionSharedModel = gameSessionSharedModel;
        }
    }
}
