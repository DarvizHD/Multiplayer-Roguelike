using Runtime.Core;
using Runtime.UI.Menu.Navigation.Screens;
using Runtime.UI.Menu.Panels.JoinLobbyPanel;
using Runtime.UI.Menu.Panels.LobbyPanel;
using Runtime.UI.Menu.Panels.LoginPanel;
using Runtime.UI.Menu.Panels.StartMenuPanel;
using Runtime.UI.Menu.Panels.UsersPanel;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Navigation
{
    public class StartMenuPresenter : IPresenter
    {
        private readonly VisualElement _root;

        public StartMenuPresenter(Router router, UICoreModel uiCoreModel, WorldViewDescription worldViewDescription, UIDocument document, UIAudioService audioService)
        {
            _root = document.rootVisualElement.Q<VisualElement>("menu-root");
            var menuContent = _root.Q<VisualElement>("menu-content");

            var loginView = new LoginPanelView(worldViewDescription.UI.Get(uiCoreModel.LoginPanelModel.ViewId).Asset, menuContent);
            var loginPresenter = new LoginPanelPresenter(uiCoreModel.LoginPanelModel, loginView, uiCoreModel, audioService);
            router.Register(ScreenIds.Login, new LoginScreenPresenter(router, loginPresenter, uiCoreModel.LoginPanelModel));

            var startMenuView = new StartMenuPanelView(worldViewDescription.UI.Get(uiCoreModel.StartMenuPanelModel.ViewId).Asset, menuContent);
            var startMenuPresenter = new StartMenuPanelPresenter(uiCoreModel.StartMenuPanelModel, startMenuView, audioService);
            router.Register(ScreenIds.StartMenu, new StartMenuScreen(router, startMenuPresenter, uiCoreModel.StartMenuPanelModel, uiCoreModel));

            var lobbyView = new LobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.LobbyPanelModel.ViewId).Asset, menuContent);
            var lobbyPresenter = new LobbyPanelPresenter(uiCoreModel.LobbyPanelModel, lobbyView, uiCoreModel, audioService);
            var usersView = new UsersPanelView(worldViewDescription.UI.Get(uiCoreModel.UsersPanelModel.ViewId).Asset, menuContent);
            var usersPresenter = new UsersPanelPresenter(usersView, worldViewDescription, uiCoreModel, audioService);
            router.Register(ScreenIds.Lobby, new LobbyScreen(router, lobbyPresenter, usersPresenter, uiCoreModel.LobbyPanelModel, uiCoreModel, audioService));

            var joinLobbyView = new JoinLobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.JoinLobbyPanelModel.ViewId).Asset, menuContent);
            var joinLobbyPresenter = new JoinLobbyPanelPresenter(uiCoreModel.JoinLobbyPanelModel, joinLobbyView, audioService);
            router.Register(ScreenIds.JoinLobby, new JoinLobbyScreen(router, joinLobbyPresenter, uiCoreModel.JoinLobbyPanelModel, uiCoreModel));
        }

        public void Enable()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        public void Disable()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}
