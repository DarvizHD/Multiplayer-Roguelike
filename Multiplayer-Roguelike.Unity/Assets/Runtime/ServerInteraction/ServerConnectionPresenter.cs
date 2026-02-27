using ENet;
using Runtime.GameSystems;

namespace Runtime.ServerInteraction
{
    public class ServerConnectionPresenter : IPresenter
    {
        private readonly ServerConnectionModel _model;
        private readonly GameSystemCollection _systemCollection;
        private ServerPlayerConnectionSystem _serverConnectionSystem;

        public ServerConnectionPresenter(ServerConnectionModel serverConnectionModel, GameSystemCollection gameSystemCollection)
        {
            _model = serverConnectionModel;
            _systemCollection = gameSystemCollection;
        }

        public void Enable()
        {
            _model.PlayerConnect += OnPlayerConnect;
            _model.PlayerDisconnect += OnPlayerDisconnect;
        }

        public void Disable()
        {
            _model.PlayerConnect -= OnPlayerConnect;
            _model.PlayerDisconnect -= OnPlayerDisconnect;
        }

        private void OnPlayerConnect()
        {
            var address = new Address();
            address.SetHost("127.0.0.1");
            address.Port = 7777;

            _model.PlayerHost = new Host();
            _model.PlayerHost.Create();

            _model.PlayerPeer = _model.PlayerHost.Connect(address, 2);

            _serverConnectionSystem = new ServerPlayerConnectionSystem(_model);
            _systemCollection.Add(_serverConnectionSystem);
        }

        private void OnPlayerDisconnect()
        {
            _model.PlayerHost.Dispose();
            _model.PlayerHost = null;
            _systemCollection.Remove(_serverConnectionSystem);
        }
    }
}
