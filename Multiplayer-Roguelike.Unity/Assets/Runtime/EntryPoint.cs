using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using ENet;
using Runtime.GameSystems;
using Runtime.ServerInteraction;
using Shared.Commands;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECS.Core.ECS Ecs { get; } = new ();
        private readonly GameSystemCollection _gameFixedSystemCollection = new();


        private async void Start()
        {
            for (var i = 0; i < 10; i++)
            {
                Ecs.AddEntityComponent(i, new PositionComponent(Random.insideUnitSphere));
                Ecs.AddEntityComponent(i, new DirectionComponent(Random.insideUnitSphere.normalized));
                Ecs.AddEntityComponent(i, new SpeedComponent(Random.value * 5f));
                Ecs.AddEntityComponent(i, new TransformComponent(GameObject.CreatePrimitive(PrimitiveType.Sphere).transform));
            }

            Ecs.AddSystem<MovementSystem>();
            Ecs.AddSystem<PatrolSystem>();
            Ecs.AddSystem<DrawTransformSystem>();
            
            Library.Initialize();
            
            var serverConnectionModel = new ServerConnectionModel();
            var serverConnectionPresenter = new ServerConnectionPresenter(serverConnectionModel, _gameFixedSystemCollection);
            serverConnectionPresenter.Enable();
            
            serverConnectionModel.ConnectPlayer();
            await serverConnectionModel.CompletePlayerConnectAwaiter;

            var loginCommand = new LoginCommand("Varfolomey");
            loginCommand.Write(serverConnectionModel.PlayerPeer);
            
            var createLobbyCommand = new CreateLobbyCommand("Varfolomey");
            createLobbyCommand.Write(serverConnectionModel.PlayerPeer);
        }

        private void Update()
        {
            Ecs.Update(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _gameFixedSystemCollection.Update(Time.fixedDeltaTime);
        }
    }
}