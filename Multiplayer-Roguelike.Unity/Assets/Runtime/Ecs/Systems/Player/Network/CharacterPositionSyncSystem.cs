using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Tools;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class CharacterPositionSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CharacterNetworkSyncComponent, PositionComponent,
            PositionInterpolationComponent, DirectionComponent, NetworkControllableTag> _buffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
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
