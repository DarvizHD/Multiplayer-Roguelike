using ENet;
using Runtime.Core;
using Runtime.GameSystems;
using Shared.Commands.Player;

namespace Runtime.ServerInteraction
{
    public class ServerConnectionPresenter : IPresenter
    {
        private readonly ServerConnectionModel _model;
        private readonly GameSystemCollection _systemCollection;
        private ServerPlayerConnectionSystem _serverConnectionSystem;

        public ServerConnectionPresenter(ServerConnectionModel serverConnectionModel,
            GameSystemCollection gameSystemCollection)
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

        private async void OnPlayerConnect(string value, string nickname)
        {
            var address = new Address();
            address.SetHost(value);
            address.Port = 7777;

            _model.PlayerHost = new Host();
            _model.PlayerHost.Create();

            _model.PlayerPeer = _model.PlayerHost.Connect(address, 2);

            if (!_systemCollection.Has("server_player_connection_system"))
            {
                _serverConnectionSystem = new ServerPlayerConnectionSystem(_model);
                _systemCollection.Add(_serverConnectionSystem);
            }

            await _model.CompletePlayerConnectAwaiter;

            var loginCommand = new LoginCommand(nickname);
            loginCommand.Write(_model.PlayerPeer);
        }

        private void OnPlayerDisconnect()
        {
            _model.PlayerHost.Dispose();
            _model.PlayerHost = null;
            _systemCollection.Remove(_serverConnectionSystem);
        }
    }
}
