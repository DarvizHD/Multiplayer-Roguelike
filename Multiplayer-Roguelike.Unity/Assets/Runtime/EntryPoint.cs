using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Runtime.UI;
using Runtime.UI.Navigation;
using Shared.Models;
using Shared.Protocol;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private WorldViewDescription worldViewDescription;
        [SerializeField] private UIDocument document;
        private readonly GameSystemCollection _gameFixedSystemCollection = new();

        private readonly UICoreModel _uiCoreModel = new();

        private GameSession _gameSession;

        private GameSessionSharedModel _gameSessionSharedModel;
        private PlayerSharedModel _playerSharedModel;

        private ServerConnectionModel _serverConnectionModel;
        private ServerConnectionPresenter _serverConnectionPresenter;

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

            var navigationPresenter = new NavigationPresenter(_uiCoreModel, worldViewDescription, document);
            navigationPresenter.Enable();

            _gameSession = new GameSession(_gameSessionSharedModel, _playerSharedModel, _serverConnectionModel);
            _gameSession.Enable();

            _serverConnectionModel.WorldPacketReceived += OnWorldPacketReceived;
            _serverConnectionModel.PlayerPacketReceived += OnPlayerPacketReceived;

            _gameSessionSharedModel.IsRun.OnChange += RunSession;
        }

        private void FixedUpdate()
        {
            _gameSession?.Update(Time.fixedDeltaTime);
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }

        private void OnWorldPacketReceived(Packet packet)
        {
            var buffer = new byte[1024];
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

        private void RunSession()
        {
            _gameSession.Run();
        }
    }
}
