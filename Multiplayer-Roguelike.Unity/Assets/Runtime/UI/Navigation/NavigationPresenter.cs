using Runtime.UI.Menu;
using Runtime.UI.Navigation.Screens;
using Runtime.UI.Panels.HostLobbyPanel;
using Runtime.UI.Panels.JoinLobbyPanel;
using Runtime.UI.Panels.LoginPanel;
using Runtime.UI.Panels.StartMenuPanel;
using Runtime.UI.Panels.UsersPanel;
using UnityEngine.UIElements;

namespace Runtime.UI.Navigation
{
    public class NavigationPresenter : IPresenter
    {
        private readonly Router _router;
        private readonly UICoreModel _uiCoreModel;
        private readonly UIDocument _document;

        public NavigationPresenter(UICoreModel uiCoreModel, WorldViewDescription worldViewDescription, UIDocument document)
        {
            _uiCoreModel = uiCoreModel;
            _document = document;
            var parentRoot = document.rootVisualElement.Q<VisualElement>("menu-content");

            _router = new Router();

            var loginView = new LoginPanelView(worldViewDescription.UI.Get(uiCoreModel.LoginPanelModel.ViewId).Asset,
                parentRoot);
            var loginPresenter = new LoginPanelPresenter(uiCoreModel.LoginPanelModel, loginView, uiCoreModel);
            _router.Register(ScreenIds.Login, new LoginScreenPresenter(_router, loginPresenter, uiCoreModel.LoginPanelModel));

            var startMenuView =
                new StartMenuPanelView(worldViewDescription.UI.Get(uiCoreModel.StartMenuPanelModel.ViewId).Asset, parentRoot);
            var startMenuPresenter = new StartMenuPanelPresenter(uiCoreModel.StartMenuPanelModel, startMenuView);
            _router.Register(ScreenIds.StartMenu,
                new StartMenuScreen(_router, startMenuPresenter, uiCoreModel.StartMenuPanelModel, uiCoreModel));

            var hostLobbyView =
                new HostLobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.HostLobbyPanelModel.ViewId).Asset, parentRoot);
            var hostLobbyPresenter = new HostLobbyPanelPresenter(uiCoreModel.HostLobbyPanelModel, hostLobbyView, uiCoreModel);
            var usersView = new UsersPanelView(worldViewDescription.UI.Get(uiCoreModel.UsersPanelModel.ViewId).Asset,
                parentRoot);
            var usersPresenter = new UsersPanelPresenter(usersView, worldViewDescription, uiCoreModel);
            _router.Register(ScreenIds.HostLobby,
                new HostLobbyScreen(_router, hostLobbyPresenter, usersPresenter, uiCoreModel.HostLobbyPanelModel, uiCoreModel));

            var joinLobbyView =
                new JoinLobbyPanelView(worldViewDescription.UI.Get(uiCoreModel.JoinLobbyPanelModel.ViewId).Asset, parentRoot);
            var joinLobbyPresenter = new JoinLobbyPanelPresenter(uiCoreModel.JoinLobbyPanelModel, joinLobbyView, uiCoreModel);
            _router.Register(ScreenIds.JoinLobby,
                new JoinLobbyScreen(_router, joinLobbyPresenter, uiCoreModel.JoinLobbyPanelModel, uiCoreModel));
        }

        public void Enable()
        {
            _document.rootVisualElement.style.display = DisplayStyle.Flex;
            _router.NavigateTo(ScreenIds.Login);

            _uiCoreModel.GameSessionSharedModel.IsRun.OnChange += Disable;
        }

        public void Disable()
        {
            _uiCoreModel.GameSessionSharedModel.IsRun.OnChange -= Disable;

            _document.rootVisualElement.style.display = DisplayStyle.None;
            _router.Clear();
        }
    }
}
