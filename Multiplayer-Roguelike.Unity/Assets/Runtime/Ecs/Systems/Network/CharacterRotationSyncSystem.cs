using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using UnityEngine;

namespace Runtime.Ecs.Systems.Network
{
    public class CharacterRotationSyncSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, characterSharedModelComponent, rotationComponent, rotationSpeedComponent, _)
                     in ComponentManager.Query<CharacterNetworkSyncComponent, RotationComponent, RotationSpeedComponent, NetworkControllableTag>())
            {
                rotationComponent.Angle = Mathf.LerpAngle
                (
                    rotationComponent.Angle,
                    characterSharedModelComponent.CharacterSharedModel.Rotation.Value,
                    rotationSpeedComponent.Speed * deltaTime
                );
            }
        }
    }
}
