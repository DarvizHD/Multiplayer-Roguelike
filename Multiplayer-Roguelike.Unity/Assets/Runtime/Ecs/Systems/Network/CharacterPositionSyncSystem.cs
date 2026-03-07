using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Tools;

namespace Runtime.ECS.Systems.Network
{
    public class CharacterPositionSyncSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, PositionComponent,
            PositionInterpolationComponent, DirectionComponent, NetworkControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterSharedModelComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];
                var interpolationComponent = _buffer.Components3[i];
                var directionComponent = _buffer.Components4[i];

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
