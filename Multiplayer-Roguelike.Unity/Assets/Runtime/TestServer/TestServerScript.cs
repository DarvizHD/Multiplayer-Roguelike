using System;
using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Runtime.TestServer;
using Shared.Commands;
using Shared.Protocol;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vector3 = Shared.Primitives.Vector3;

public class TestServerScript : MonoBehaviour
{
    [SerializeField] private TestCharacterModel _characterPrefab;

    private readonly GameSystemCollection _gameFixedSystemCollection = new();
    private ServerConnectionModel _serverConnectionModel;

    private TestPlayerModel _player;
    private TestWorldModel _world;

    private async void Start()
    {
        _serverConnectionModel = new ServerConnectionModel();
        var serverConnectionPresenter =
            new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
        serverConnectionPresenter.Enable();

        _serverConnectionModel.ConnectPlayer();
        await _serverConnectionModel.CompletePlayerConnectAwaiter;

        _playerControl = new PlayerControls();
        _playerControl.Enable();

        _player = new TestPlayerModel();
        _world = new TestWorldModel();

        _serverConnectionModel.PlayerPacketReceived += OnPlayerPacketReceived;
        _serverConnectionModel.WorldPacketReceived += OnWorldPacketReceived;
    }

    private void OnPlayerPacketReceived(Packet packet)
    {
        var buffer = new byte[1024];
        packet.CopyTo(buffer);

        var protocol = new NetworkProtocol(buffer);
        protocol.Get(out string id);
        _player.PlayerSharedModel.Read(protocol);

        lobbyID = _player.PlayerSharedModel.Lobby.LobbyId.Value;
    }

    private void OnWorldPacketReceived(Packet packet)
    {
        var buffer = new byte[1024];
        packet.CopyTo(buffer);

        var protocol = new NetworkProtocol(buffer);
        protocol.Get(out string id);
        _world.World.Read(protocol);

        foreach (var characterSharedModel in _world.World.Characters.Models)
        {
            if (!_world.Characters.ContainsKey(characterSharedModel.Id))
            {
                var newCharacter = Instantiate(_characterPrefab, UnityEngine.Vector3.zero, Quaternion.identity);
                newCharacter.Model = characterSharedModel;
                newCharacter.NameText.text = characterSharedModel.Id;
                _world.Characters.Add(characterSharedModel.Id, newCharacter);
            }
        }
    }

    private Vector2 _lastInput = Vector2.zero;
    private PlayerControls _playerControl;

    private void Update()
    {
        if (_playerControl == null) return;

        var moveInput = _playerControl.Gameplay.Move.ReadValue<Vector2>();
        if ((moveInput - _lastInput).sqrMagnitude > 0.1f)
        {
            OnMoved(moveInput);
            _lastInput = moveInput;
        }
    }

    private void FixedUpdate()
    {
        _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
    }

    private Rect windowRect;
    private int windowWidth = 400;
    private int windowHeight = 220;

    private string nickname = "";
    private string lobbyID = "";

    private void OnGUI()
    {
        windowRect = new Rect(
            Screen.width - windowWidth - 15,
            15,
            windowWidth,
            windowHeight
        );

        windowRect = GUI.Window(0, windowRect, DrawWindow, "");
    }

    private void DrawWindow(int id)
    {
        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Nickname:", GUILayout.Width(80));
        nickname = GUILayout.TextField(nickname, GUILayout.Width(250));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Lobby ID:", GUILayout.Width(80));
        lobbyID = GUILayout.TextField(lobbyID, GUILayout.Width(250));
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
        var loginCommand = new LoginCommand(nickname);
        loginCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    public void CreateLobby()
    {
        var createLobbyCommand = new CreateLobbyCommand(nickname);
        createLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    public void JoinLobby()
    {
        var joinLobbyCommand = new JoinLobbyCommand(lobbyID, nickname);
        joinLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    public void StartSession()
    {
        var startSessionCommand = new StartSessionCommand(nickname, lobbyID);
        startSessionCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    private void OnMoved(Vector2 moveInput)
    {
        var direction = new Vector3(moveInput.x, 0, moveInput.y);

        var moveCommand = new MoveCommand(nickname, direction);
        moveCommand.Write(_serverConnectionModel.PlayerPeer);
    }
}
