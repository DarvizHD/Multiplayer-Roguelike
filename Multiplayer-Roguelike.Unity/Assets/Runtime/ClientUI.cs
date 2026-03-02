using Runtime.ServerInteraction;
using Shared.Commands;
using UnityEngine;

namespace Runtime
{
    public class ClientUI : MonoBehaviour
    {
        private ClientModel _clientModel;

        private ServerConnectionModel _serverConnectionModel;
        private GameSession _gameSession;

        private Rect _windowRect;
        private readonly int _windowHeight = 240;
        private readonly int _windowWidth = 480;

        public void Construct(ClientModel clientModel, ServerConnectionModel serverConnectionModel, GameSession gameSession)
        {
            _clientModel =  clientModel;

            _serverConnectionModel = serverConnectionModel;

            _gameSession = gameSession;
        }

        private void OnGUI()
        {
            if (_clientModel == null)
            {
                return;
            }

            _windowRect = new Rect(
                Screen.width - _windowWidth - 15,
                15,
                _windowWidth,
                _windowHeight
            );

            _windowRect = GUI.Window(0, _windowRect, DrawWindow, "");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Nickname:", GUILayout.Width(80));
            _clientModel.Nickname = GUILayout.TextField(_clientModel.Nickname, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Lobby ID:", GUILayout.Width(80));
            _clientModel.LobbyId = GUILayout.TextField(_clientModel.LobbyId, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Enter", GUILayout.Height(35)))
            {
                ConnectPlayer();
            }

            if (GUILayout.Button("Create Lobby", GUILayout.Height(35)))
            {
                CreateLobby();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start", GUILayout.Height(35)))
            {
                StartSession();
            }

            if (GUILayout.Button("Join Lobby", GUILayout.Height(35)))
            {
                JoinLobby();
            }
            GUILayout.EndHorizontal();
        }

        public void ConnectPlayer()
        {
            var loginCommand = new LoginCommand(_clientModel.Nickname);
            loginCommand.Write(_serverConnectionModel.PlayerPeer);
        }

        public void CreateLobby()
        {
            var createLobbyCommand = new CreateLobbyCommand(_clientModel.Nickname);
            createLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
        }

        public void JoinLobby()
        {
            var joinLobbyCommand = new JoinLobbyCommand(_clientModel.Nickname, _clientModel.LobbyId);
            joinLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
        }

        public void StartSession()
        {
            var startSessionCommand = new StartSessionCommand(_clientModel.Nickname, _clientModel.LobbyId);
            startSessionCommand.Write(_serverConnectionModel.PlayerPeer);
        }
    }
}
