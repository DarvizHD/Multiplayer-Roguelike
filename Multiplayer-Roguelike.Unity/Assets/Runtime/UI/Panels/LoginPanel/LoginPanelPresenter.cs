using Shared.Commands.Player;

namespace Runtime.UI.Panels.LoginPanel
{
    public class LoginPanelPresenter : IPresenter
    {
        private readonly LoginPanelView _view;
        private readonly LoginPanelModel _model;
        private readonly UICoreModel _uiCoreModel;

        public LoginPanelPresenter(LoginPanelModel model, LoginPanelView view, UICoreModel uiCoreModel)
        {
            _model = model;
            _view = view;
            _uiCoreModel = uiCoreModel;
        }

        public void Enable()
        {
            _view.ParentRoot.Add(_view.Root);
            _view.ConfirmButton.clicked += OnConfirmButtonClicked;

            _uiCoreModel.PlayerSharedModel.Nickname.OnChange += OnNicknameChanged;
        }

        private void OnNicknameChanged()
        {
            _model.SetUsername(_uiCoreModel.PlayerSharedModel.Nickname.Value);
            _model.Confirm();
        }

        public void Disable()
        {
            _view.ConfirmButton.clicked -= OnConfirmButtonClicked;
            _view.ParentRoot.Remove(_view.Root);
        }

        private void OnConfirmButtonClicked()
        {
            if (string.IsNullOrEmpty(_view.UsernameTextField.value))
            {
                return;
            }

            var loginCommand = new LoginCommand(_view.UsernameTextField.value);
            loginCommand.Write(_uiCoreModel.ServerConnectionModel.PlayerPeer);
        }
    }
}
