using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.LoginPanel
{
    public class LoginPanelPresenter : BasePanelPresenter
    {
        protected override VisualElement Root => _view.Root;

        private readonly LoginPanelView _view;
        private readonly LoginPanelModel _model;
        private readonly UICoreModel _uiCoreModel;
        private const string _defaultAddress = "127.0.0.1";
        private const string _selectedClass = "selected";

        private bool _isOnline;

        public LoginPanelPresenter(LoginPanelModel model, LoginPanelView view, UICoreModel uiCoreModel, UIAudioService audioService) : base(audioService)
        {
            _model = model;
            _view = view;
            _uiCoreModel = uiCoreModel;
        }

        public override void Enable()
        {
            _view.ParentRoot.Add(_view.Root);

            base.Enable();

            _view.ConfirmButton.clicked += OnConfirmButtonClicked;
            _view.OnlineButton.clicked += OnOnlineButtonClicked;
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged += OnNicknameChanged;
            _view.AddressContainer.style.display = DisplayStyle.None;
        }

        private void OnOnlineButtonClicked()
        {
            _isOnline = !_isOnline;

            if (_isOnline)
            {
                _view.OnlineButton.AddToClassList(_selectedClass);
            }
            else
            {
                _view.OnlineButton.RemoveFromClassList(_selectedClass);
            }

            _view.AddressContainer.style.display = _isOnline ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnNicknameChanged(string value)
        {
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged -= OnNicknameChanged;
            _model.SetUsername(value);
        }

        public override void Disable()
        {
            _view.ConfirmButton.clicked -= OnConfirmButtonClicked;
            _view.OnlineButton.clicked -= OnOnlineButtonClicked;
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged -= OnNicknameChanged;
            _view.Root.RemoveFromHierarchy();

            base.Disable();
        }

        private async void OnConfirmButtonClicked()
        {
            if (string.IsNullOrEmpty(_view.UsernameTextField.value))
            {
                return;
            }

            if (!_isOnline)
            {
                _uiCoreModel.ServerConnectionModel.ConnectPlayer(_defaultAddress, _view.UsernameTextField.value);
                await _uiCoreModel.ServerConnectionModel.CompletePlayerConnectAwaiter;
                _model.Confirm();
                return;
            }

            if (string.IsNullOrEmpty(_view.AddressTextField.value))
            {
                return;
            }

            _uiCoreModel.ServerConnectionModel.ConnectPlayer(_view.AddressTextField.value, _view.UsernameTextField.value);
            await _uiCoreModel.ServerConnectionModel.CompletePlayerConnectAwaiter;
            _model.Confirm();
        }
    }
}
