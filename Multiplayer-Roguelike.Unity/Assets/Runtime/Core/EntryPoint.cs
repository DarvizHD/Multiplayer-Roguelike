using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Runtime.Sound;
using Runtime.Sound.Constants;
using Runtime.UI;
using Runtime.UI.HUD;
using Runtime.UI.Menu;
using Runtime.UI.Menu.Navigation;
using Runtime.UI.Menu.Parallax;
using Runtime.ViewDescriptions;
using Shared.Commands.Player;
using Shared.Models.GameSession;
using Shared.Models.Player;
using Shared.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private WorldViewDescription _worldViewDescription;
        [SerializeField] private UIDocument _menuDocument;
        [SerializeField] private UIHudView _uiHudView;
        [SerializeField] private AudioSource _ambientSoundSource;
        [SerializeField] private Camera _dustRenderCamera;

        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private readonly UICoreModel _uiCoreModel = new();

        private GameSession _gameSession;

        private GameSessionSharedModel _gameSessionSharedModel;
        private PlayerSharedModel _playerSharedModel;

        private ServerConnectionModel _serverConnectionModel;
        private ServerConnectionPresenter _serverConnectionPresenter;

        private Router _router;
        private UIPresenter _uiPresenter;

        private AmbientSoundPresenter _ambientSoundPresenter;
        private ParallaxPresenter _parallaxPresenter;
        private DustParticlePresenter _dustParticlePresenter;
        private SoundPresenter _soundPresenter;
        private SoundModel _soundModel;

        private void Start()
        {
            Application.runInBackground = true;

            _playerSharedModel = new PlayerSharedModel(string.Empty);
            _gameSessionSharedModel = new GameSessionSharedModel(string.Empty);

            Library.Initialize();

            _serverConnectionModel = new ServerConnectionModel();
            _serverConnectionPresenter =
                new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
            _serverConnectionPresenter.Enable();

            _uiCoreModel.Setup(_playerSharedModel, _serverConnectionModel, _gameSessionSharedModel);

            var audioSource = _menuDocument.GetComponent<AudioSource>();
            var buttonClickClip = Resources.Load<AudioClip>(AudioResourcesConstants.Menu.ButtonClick);
            var joinToLobbyClip = Resources.Load<AudioClip>(AudioResourcesConstants.Menu.JoinToLobby);

            var uiAudioService = new UIAudioService(audioSource, buttonClickClip, joinToLobbyClip);

            _router = new Router();
            _uiPresenter = new UIPresenter(_router, _uiCoreModel, _worldViewDescription, _menuDocument, uiAudioService, _uiHudView);
            _uiPresenter.Enable();

            var parallaxView = new ParallaxView(_menuDocument);
            _parallaxPresenter = new ParallaxPresenter(parallaxView);
            _parallaxPresenter.Enable();

            _dustParticlePresenter = new DustParticlePresenter(_dustRenderCamera);
            _dustParticlePresenter.Enable();

            _soundModel = new SoundModel();
            _soundPresenter = new SoundPresenter(_soundModel, this);
            _soundPresenter.Enable();

            _ambientSoundPresenter = new AmbientSoundPresenter(_soundModel, _gameSessionSharedModel);
            _ambientSoundPresenter.Enable();

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
            _gameSession?.FixedUpdate(Time.fixedDeltaTime);
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }

        private void Update()
        {
            _gameSession?.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _gameSession?.LateUpdate(Time.deltaTime);
        }

        private void OnWorldPacketReceived(Packet packet)
        {
            var buffer = new byte[packet.Length];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string _);

            _gameSessionSharedModel.Read(protocol);
        }

        private void OnPlayerPacketReceived(Packet packet)
        {
            var buffer = new byte[packet.Length];
            packet.CopyTo(buffer);

            var protocol = new NetworkProtocol(buffer);
            protocol.Get(out string _);
            _playerSharedModel.Read(protocol);
        }

        private void RunSession(bool value)
        {
            if (value)
            {
                _parallaxPresenter.Disable();
                _dustParticlePresenter.Disable();

                _gameSession = new GameSession(_gameSessionSharedModel, _playerSharedModel, _serverConnectionModel, _uiHudView, _worldViewDescription, _soundModel);
                _gameSession.Enable();
                _gameSession.Run();
            }
            else
            {
                _parallaxPresenter.Enable();
                _dustParticlePresenter.Enable();

                _gameSession.Stop();
                _gameSession.Disable();
                _gameSession = null;
            }
        }
    }
}
