using System;
using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Runtime.TestServer;
using Shared.Commands;
using Shared.Protocol;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector3 = Shared.Primitives.Vector3;

public class TestServerScript : MonoBehaviour
{
    [SerializeField] private TestCharacterModel _character;

    private readonly GameSystemCollection _gameFixedSystemCollection = new();
    private ServerConnectionModel _serverConnectionModel;

    private TestPlayerModel _player;

    private async void Start()
    {
        _serverConnectionModel = new ServerConnectionModel();
        var serverConnectionPresenter = new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
        serverConnectionPresenter.Enable();

        _serverConnectionModel.ConnectPlayer();
        await _serverConnectionModel.CompletePlayerConnectAwaiter;

        _playerControl = new PlayerControls();
        _playerControl.Enable();

        _player = new TestPlayerModel();
        _character.Model = _player.PlayerSharedModel.Character;

        _serverConnectionModel.PacketReceived += OnPacketReceived;
    }

    private void OnPacketReceived(Packet packet)
    {
        var buffer = new byte[1024];
        packet.CopyTo(buffer);

        var protocol = new NetworkProtocol(buffer);
        protocol.Get(out string id);
        _player.PlayerSharedModel.Read(protocol);
    }

    private void OnMoved(Vector2 moveInput)
    {
        var direction = new Vector3(moveInput.x, 0, moveInput.y);

        var moveCommand = new MoveCommand(_nicknameInputField.text, direction);
        moveCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    private Vector2 _lastInput = Vector2.zero;

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

    [SerializeField] private InputField _nicknameInputField;
    [SerializeField] private InputField _lobbyIdInputField;
    private PlayerControls _playerControl;

    public void ConnectPlayer()
    {
        var loginCommand = new LoginCommand(_nicknameInputField.text);
        loginCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    public void CreateLobby()
    {
        var createLobbyCommand = new CreateLobbyCommand(_nicknameInputField.text);
        createLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
    }

    public void JoinLobby()
    {
        var joinLobbyCommand = new JoinLobbyCommand(_lobbyIdInputField.text, _nicknameInputField.text);
        joinLobbyCommand.Write(_serverConnectionModel.PlayerPeer);
    }
}
