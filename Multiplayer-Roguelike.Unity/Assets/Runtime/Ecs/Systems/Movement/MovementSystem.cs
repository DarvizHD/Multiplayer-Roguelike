using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Movement
{
    public class MovementSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, DirectionComponent, MoveSpeedComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var positionComponent = _buffer.Components1[i];
                var directionComponent = _buffer.Components2[i];
                var moveSpeedComponent = _buffer.Components3[i];

                if (ComponentManager.HasComponent<DeathTagComponent>(entityId) ||
                    ComponentManager.HasComponent<DeathAnimationComponent>(entityId))
                {
                    return;
                }

                positionComponent.Position += directionComponent.Direction.normalized * (moveSpeedComponent.Speed * deltaTime);
            }
        }
    }
}
