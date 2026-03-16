using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Tools;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class CharacterPositionSendSystem : BaseSystem
    {
        private const float _positionThreshold = 0.1f;
        private const float _positionThresholdSqr = _positionThreshold * _positionThreshold;

        private QueryBuffer<CharacterConnectionComponent, CharacterNetworkSyncComponent,
            PositionComponent, DirectionComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterConnectionComponent = _buffer.Components1[i];
                var characterNetworkSyncComponent = _buffer.Components2[i];
                var positionComponent = _buffer.Components3[i];
                var directionComponent = _buffer.Components4[i];

                var deltaPosition = positionComponent.Position - characterNetworkSyncComponent.CharacterSharedModel.Position.Value.ToUnityVector3();

                if (deltaPosition.sqrMagnitude < _positionThresholdSqr)
                {
                    continue;
                }

                var moveCommand = new MoveCommand
                (
                    characterNetworkSyncComponent.CharacterSharedModel.Id,
                    positionComponent.Position.ToSharedVector3(),
                    directionComponent.Direction.ToSharedVector3()
                );

                moveCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
