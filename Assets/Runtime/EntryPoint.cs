using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Server.Commands;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        private readonly GameSystemCollection _gameFixedSystemCollection = new();
        
        private async void Start()
        {
            Library.Initialize();
            
            var serverConnectionModel = new ServerConnectionModel();
            var serverConnectionPresenter = new ServerConnectionPresenter(serverConnectionModel, _gameFixedSystemCollection);
            serverConnectionPresenter.Enable();
            
            serverConnectionModel.ConnectPlayer();
            await serverConnectionModel.CompletePlayerConnectAwaiter;

            var loginCommand = new LoginCommand("Varfolomey");
            loginCommand.Write(serverConnectionModel.PlayerPeer);
        }

        private void FixedUpdate()
        {
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }
    }
}