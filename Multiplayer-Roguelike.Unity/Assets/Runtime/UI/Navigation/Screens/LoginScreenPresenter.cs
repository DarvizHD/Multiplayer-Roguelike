using Runtime.UI.Menu;
using Runtime.UI.Panels.LoginPanel;

namespace Runtime.UI.Navigation.Screens
{
    public class LoginScreenPresenter : IPresenter
    {
        private readonly Router _router;
        private readonly LoginPanelPresenter _presenter;
        private readonly LoginPanelModel _model;

        public LoginScreenPresenter(Router router, LoginPanelPresenter presenter, LoginPanelModel model)
        {
            _router = router;
            _presenter = presenter;
            _model = model;
        }

        public void Enable()
        {
            _model.OnConfirmed += HandleConfirmed;
            _presenter.Enable();
        }

        public void Disable()
        {
            _model.OnConfirmed -= HandleConfirmed;
            _presenter.Disable();
        }

        private void HandleConfirmed()
        {
            _router.NavigateTo(ScreenIds.StartMenu);
        }
    }
}
