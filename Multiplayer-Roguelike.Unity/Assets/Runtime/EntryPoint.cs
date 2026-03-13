using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Runtime.Sound;
using Runtime.UI;
using Runtime.UI.HUD;
using Runtime.UI.Navigation;
using Runtime.UI.Parallax;
using Shared.Commands.Player;
using Shared.Models.GameSession;
using Shared.Models.Player;
using Shared.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private WorldViewDescription _worldViewDescription;
        [SerializeField] private UIDocument _menuDocument;
        [SerializeField] private UIHudView _uiHudView;
        [SerializeField] private AudioSource _ambientSoundSource;

        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private readonly UICoreModel _uiCoreModel = new();

        private GameSession _gameSession;

        private GameSessionSharedModel _gameSessionSharedModel;
        private PlayerSharedModel _playerSharedModel;

        private ServerConnectionModel _serverConnectionModel;
        private ServerConnectionPresenter _serverConnectionPresenter;

        private ParallaxPresenter _parallaxPresenter;
        private NavigationPresenter _navigationPresenter;
        private AmbientSoundPresenter _ambientSoundPresenter;

        private async void Start()
        {
            Application.runInBackground = true;

            _playerSharedModel = new PlayerSharedModel(string.Empty);
            _gameSessionSharedModel = new GameSessionSharedModel(string.Empty);

            Library.Initialize();

            _serverConnectionModel = new ServerConnectionModel();
            _serverConnectionPresenter =
                new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
            _serverConnectionPresenter.Enable();

            _serverConnectionModel.ConnectPlayer();
            await _serverConnectionModel.CompletePlayerConnectAwaiter;

            _uiCoreModel.Setup(_playerSharedModel, _serverConnectionModel, _gameSessionSharedModel);

            var audioSource = _menuDocument.GetComponent<AudioSource>();
            var buttonClickClip = Resources.Load<AudioClip>(AudioResourcesConstants.Menu.ButtonClick);
            var joinToLobbyClip = Resources.Load<AudioClip>(AudioResourcesConstants.Menu.JoinToLobby);

            var uiAudioService = new UIAudioService(audioSource, buttonClickClip, joinToLobbyClip);

            _navigationPresenter = new NavigationPresenter(_uiCoreModel, _worldViewDescription, _menuDocument, uiAudioService);
            _navigationPresenter.Enable();

            var parallaxView = new ParallaxView(_menuDocument.rootVisualElement);
            _parallaxPresenter = new ParallaxPresenter(parallaxView);
            _parallaxPresenter.Enable();

            _ambientSoundPresenter = new AmbientSoundPresenter(_ambientSoundSource, _gameSessionSharedModel);
            _ambientSoundPresenter.Enable();

            _gameSession = new GameSession(_gameSessionSharedModel, _playerSharedModel, _serverConnectionModel, _uiHudView, _worldViewDescription);
            _gameSession.Enable();

            _serverConnectionModel.WorldPacketReceived += OnWorldPacketReceived;
            _serverConnectionModel.PlayerPacketReceived += OnPlayerPacketReceived;

            _gameSessionSharedModel.IsRun.OnChanged += RunSession;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

#if UNITY_EDITOR
        private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                var commandLogout = new LogoutCommand(_playerSharedModel.Nickname.Value);
                commandLogout.Write(_serverConnectionModel.PlayerPeer);
            }
        }
#endif

        private void FixedUpdate()
        {
            _gameSession?.Update(Time.fixedDeltaTime);
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }

        private void OnWorldPacketReceived(Packet packet)
        {
            var buffer = new byte[packet.Length];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string id);

            _gameSessionSharedModel.Read(protocol);
        }

        private void OnPlayerPacketReceived(Packet packet)
        {
            var buffer = new byte[1024];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string id);
            _playerSharedModel.Read(protocol);
        }

        private void RunSession(bool value)
        {
            _parallaxPresenter.Disable();
            _navigationPresenter.Disable();

            _gameSession.Run();
        }
    }
}
