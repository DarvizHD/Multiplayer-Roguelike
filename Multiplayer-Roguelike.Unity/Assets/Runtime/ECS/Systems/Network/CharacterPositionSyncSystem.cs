using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Network;
using UnityEngine;

namespace Runtime.ECS.Systems.Network
{
    public class CharacterPositionSyncSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, characterSharedModelComponent, positionComponent, interpolationComponent, directionComponent, _)
                     in ComponentManager.Query<CharacterNetworkSyncComponent, PositionComponent, PositionInterpolationComponent, DirectionComponent, NetworkControllableTag>())
            {
                if (characterSharedModelComponent.CharacterSharedModel.Position.IsDirty)
                {
                    interpolationComponent.LastTime = interpolationComponent.TargetTime;
                    interpolationComponent.TargetTime = interpolationComponent.TotalTime;

                    interpolationComponent.LastPosition = positionComponent.Position;
                    interpolationComponent.TargetPosition = characterSharedModelComponent.CharacterSharedModel.Position
                        .Value.ToUnityVector3();

                    characterSharedModelComponent.CharacterSharedModel.Position.ClearDirty();
                }

                if (characterSharedModelComponent.CharacterSharedModel.Direction.IsDirty)
                {
                    directionComponent.Direction = characterSharedModelComponent.CharacterSharedModel.Direction.Value.ToUnityVector3();

                    characterSharedModelComponent.CharacterSharedModel.Direction.ClearDirty();
                }
            }
        }
    }
}
