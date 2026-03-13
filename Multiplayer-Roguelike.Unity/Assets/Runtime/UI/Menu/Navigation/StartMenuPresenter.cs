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
        private readonly UIDocument _document;

        public StartMenuPresenter(Router router, UICoreModel uiCoreModel, WorldViewDescription worldViewDescription, UIDocument document, UIAudioService audioService)
        {
            _document = document;

            var contentRoot = document.rootVisualElement.Q<VisualElement>("menu-content");

            var loginView = new LoginPanelView(worldViewDescription.UI.Get(uiCoreModel.LoginPanelModel.ViewId).Asset, contentRoot);
            var loginPresenter = new LoginPanelPresenter(uiCoreModel.LoginPanelModel, loginView, uiCoreModel);
            router.Register(ScreenIds.Login, new LoginScreenPresenter(router, loginPresenter, uiCoreModel.LoginPanelModel));

            var startMenuView = new StartMenuPanelView(worldViewDescription.UI.Get(uiCoreModel.StartMenuPanelModel.ViewId).Asset, contentRoot);
            var startMenuPresenter = new StartMenuPanelPresenter(uiCoreModel.StartMenuPanelModel, startMenuView);
            router.Register(ScreenIds.StartMenu, new StartMenuScreen(router, startMenuPresenter, uiCoreModel.StartMenuPanelModel, uiCoreModel, audioService));

            var lobbyView = new LobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.LobbyPanelModel.ViewId).Asset, contentRoot);
            var lobbyPresenter = new LobbyPanelPresenter(uiCoreModel.LobbyPanelModel, lobbyView, uiCoreModel);
            var usersView = new UsersPanelView(worldViewDescription.UI.Get(uiCoreModel.UsersPanelModel.ViewId).Asset, contentRoot);
            var usersPresenter = new UsersPanelPresenter(usersView, worldViewDescription, uiCoreModel, audioService);
            router.Register(ScreenIds.Lobby, new LobbyScreen(router, lobbyPresenter, usersPresenter, uiCoreModel.LobbyPanelModel, uiCoreModel, audioService));

            var joinLobbyView = new JoinLobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.JoinLobbyPanelModel.ViewId).Asset, contentRoot);
            var joinLobbyPresenter = new JoinLobbyPanelPresenter(uiCoreModel.JoinLobbyPanelModel, joinLobbyView, uiCoreModel);
            router.Register(ScreenIds.JoinLobby, new JoinLobbyScreen(router, joinLobbyPresenter, uiCoreModel.JoinLobbyPanelModel, uiCoreModel));
        }

        public void Enable()
        {
            _document.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Disable()
        {
            _document.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
