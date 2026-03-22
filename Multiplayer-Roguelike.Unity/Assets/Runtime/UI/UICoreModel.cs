using Runtime.ServerInteraction;
using Runtime.UI.Menu.Panels.JoinLobbyPanel;
using Runtime.UI.Menu.Panels.LobbyPanel;
using Runtime.UI.Menu.Panels.LoginPanel;
using Runtime.UI.Menu.Panels.StartMenuPanel;
using Runtime.UI.Menu.Panels.UsersPanel;
using Shared.Models.GameSession;
using Shared.Models.Player;

namespace Runtime.UI
{
    public class UICoreModel
    {
        public LoginPanelModel LoginPanelModel { get; } = new();
        public StartMenuPanelModel StartMenuPanelModel { get; } = new();
        public LobbyPanelModel LobbyPanelModel { get; } = new();
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
