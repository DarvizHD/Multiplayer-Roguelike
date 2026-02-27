using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Shared.Commands;
using UnityEngine;
using UnityEngine.UI;

public class TestServerScript : MonoBehaviour
{
    private readonly GameSystemCollection _gameFixedSystemCollection = new();
    private ServerConnectionModel _serverConnectionModel;

    private async void Start()
    {
        _serverConnectionModel = new ServerConnectionModel();
        var serverConnectionPresenter = new ServerConnectionPresenter(_serverConnectionModel, _gameFixedSystemCollection);
        serverConnectionPresenter.Enable();
            
        _serverConnectionModel.ConnectPlayer();
        await _serverConnectionModel.CompletePlayerConnectAwaiter;
    }
    
    private void FixedUpdate()
    {
        _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
    }
    
    [SerializeField] private InputField _nicknameInputField;
    [SerializeField] private InputField _lobbyIdInputField;
    
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
