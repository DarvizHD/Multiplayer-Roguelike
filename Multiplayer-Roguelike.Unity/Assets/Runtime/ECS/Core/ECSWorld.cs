using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using Runtime.ServerInteraction;
using Shared.Models;
using UnityEngine;

namespace Runtime.ECS.Core
{
    public class CharacterNetworkSyncComponent : IComponent
    {
        public readonly CharacterSharedModel CharacterSharedModel;

        public CharacterNetworkSyncComponent(CharacterSharedModel characterSharedModel)
        {
            CharacterSharedModel = characterSharedModel;
        }
    }

    public class CharacterConnectionComponent : IComponent
    {
        public readonly ServerConnectionModel ServerConnectionModel;

        public CharacterConnectionComponent(ServerConnectionModel serverConnectionModel)
        {
            ServerConnectionModel = serverConnectionModel;
        }
    }

    public class CharacterSharedSyncSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach ((var entityId, var characterSharedModelComponent, var positionComponent, var directionComponent, var rotationComponent, var rotationSpeedComponent)
                     in ComponentManager.Query<CharacterNetworkSyncComponent, PositionComponent, DirectionComponent, RotationComponent, RotationSpeedComponent>())
            {
                var delta = positionComponent.Position  - characterSharedModelComponent.CharacterSharedModel.LastPosition.Value.ToUnityVector3();

                if (delta.sqrMagnitude > 0.3f)
                {
                    positionComponent.Position = Vector3.Lerp(positionComponent.Position, characterSharedModelComponent.CharacterSharedModel.LastPosition.Value.ToUnityVector3(), deltaTime);
                }

                rotationComponent.Angle = Mathf.LerpAngle(rotationComponent.Angle, characterSharedModelComponent.CharacterSharedModel.Rotation.Value, rotationSpeedComponent.Speed *  deltaTime);
                directionComponent.Direction = characterSharedModelComponent.CharacterSharedModel.Direction.Value.ToUnityVector3();
            }
        }
    }

    public class ECSWorld
    {
        public ComponentManager ComponentManager { get; }
        public SystemManager SystemManager { get; }

        public ECSWorld()
        {
            ComponentManager = new ComponentManager(64);
            SystemManager = new SystemManager(ComponentManager);
        }

        public void Update(float deltaTime)
        {
            SystemManager.UpdateAll(deltaTime);
            ComponentManager.RemoveComponents();
        }

        public void RegisterComponent<T>() where T : class, IComponent
        {
            ComponentManager.RegisterComponent<T>();
        }

        public void AddEntityComponent<T>(int entityId, T component) where T : class, IComponent
        {
            ComponentManager.AddComponent(entityId, component);
        }

        public void AddSystem<T>() where T : BaseSystem, new()
        {
            SystemManager.RegisterSystem<T>();
        }
    }
}
