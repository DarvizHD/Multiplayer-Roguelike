using Runtime.Core;
using UnityEngine.UIElements;

namespace Runtime.UI.Menu.Panels.LoginPanel
{
    public class LoginPanelPresenter : IPresenter
    {
        private readonly LoginPanelView _view;
        private readonly LoginPanelModel _model;
        private readonly UICoreModel _uiCoreModel;
        private const string _defaultAddress = "127.0.0.1";

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
            _view.OnlineToggle.RegisterValueChangedCallback(OnOnlineToggleClicked);
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged += OnNicknameChanged;
        }

        private void OnOnlineToggleClicked(ChangeEvent<bool> evt)
        {
            _view.AddressContainer.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnNicknameChanged(string value)
        {
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged -= OnNicknameChanged;

            _model.SetUsername(value);
        }

        public void Disable()
        {
            _view.ConfirmButton.clicked -= OnConfirmButtonClicked;
            _uiCoreModel.PlayerSharedModel.Nickname.OnChanged -= OnNicknameChanged;
            _view.OnlineToggle.UnregisterValueChangedCallback(OnOnlineToggleClicked);
            _view.Root.RemoveFromHierarchy();
        }

        private async void OnConfirmButtonClicked()
        {
            if (string.IsNullOrEmpty(_view.UsernameTextField.value))
            {
                return;
            }

            if (!_view.OnlineToggle.value)
            {
                _uiCoreModel.ServerConnectionModel.ConnectPlayer(_defaultAddress, _view.UsernameTextField.value);
                await _uiCoreModel.ServerConnectionModel.CompletePlayerConnectAwaiter;
                _model.Confirm();
                return;
            }

            if (_view.OnlineToggle.value && string.IsNullOrEmpty(_view.AddressTextField.value))
            {
                return;
            }

            _uiCoreModel.ServerConnectionModel.ConnectPlayer(_view.AddressTextField.value, _view.UsernameTextField.value);
            await _uiCoreModel.ServerConnectionModel.CompletePlayerConnectAwaiter;
            _model.Confirm();
        }
    }
}
