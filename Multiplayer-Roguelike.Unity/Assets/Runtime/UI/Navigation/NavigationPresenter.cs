using Runtime.UI.Menu;
using Runtime.UI.Menu.Runtime.UI.Navigation;
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
        private readonly World _world;
        private readonly UIDocument _document;

        public NavigationPresenter(World world, WorldViewDescription worldViewDescription, UIDocument document)
        {
            _world = world;
            _document = document;
            var parentRoot = document.rootVisualElement.Q<VisualElement>("menu-content");

            _router = new Router();

            var loginView = new LoginPanelView(worldViewDescription.UI.Get(world.LoginPanelModel.ViewId).Asset, parentRoot);
            var loginPresenter = new LoginPanelPresenter(world.LoginPanelModel, loginView, world);
            _router.Register(ScreenIds.Login, new LoginScreenPresenter(_router, loginPresenter, world.LoginPanelModel));

            var startMenuView = new StartMenuPanelView(worldViewDescription.UI.Get(world.StartMenuPanelModel.ViewId).Asset, parentRoot);
            var startMenuPresenter = new StartMenuPanelPresenter(world.StartMenuPanelModel, startMenuView);
            _router.Register(ScreenIds.StartMenu, new StartMenuScreen(_router, startMenuPresenter, world.StartMenuPanelModel, world));

            var hostLobbyView = new HostLobbyPanelView(worldViewDescription.UI.Get(world.HostLobbyPanelModel.ViewId).Asset, parentRoot);
            var hostLobbyPresenter = new HostLobbyPanelPresenter(world.HostLobbyPanelModel, hostLobbyView, world);
            var usersView = new UsersPanelView(worldViewDescription.UI.Get(world.UsersPanelModel.ViewId).Asset, parentRoot);
            var usersPresenter = new UsersPanelPresenter(usersView, worldViewDescription, world);
            _router.Register(ScreenIds.HostLobby, new HostLobbyScreen(_router, hostLobbyPresenter, usersPresenter, world.HostLobbyPanelModel, world));

            var joinLobbyView = new JoinLobbyPanelView(worldViewDescription.UI.Get(world.JoinLobbyPanelModel.ViewId).Asset, parentRoot);
            var joinLobbyPresenter = new JoinLobbyPanelPresenter(world.JoinLobbyPanelModel, joinLobbyView, world);
            _router.Register(ScreenIds.JoinLobby, new JoinLobbyScreen(_router, joinLobbyPresenter, world.JoinLobbyPanelModel, world));
        }

        public void Enable()
        {
            _document.rootVisualElement.style.display = DisplayStyle.Flex;
            _router.NavigateTo(ScreenIds.Login);

            _world.GameSessionSharedModel.IsRun.OnChange += Disable;
        }

        public void Disable()
        {
            _world.GameSessionSharedModel.IsRun.OnChange -= Disable;

            _document.rootVisualElement.style.display = DisplayStyle.None;
            _router.Clear();
        }
    }
}
