using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Movement;

namespace Runtime.ECS.Systems.Movement
{
    public class PlayerMovementSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, DirectionComponent,
            MoveSpeedComponent, RigidbodyComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var positionComponent = _buffer.Components1[i];
                var directionComponent = _buffer.Components2[i];
                var moveSpeedComponent = _buffer.Components3[i];
                var rigidbodyComponent = _buffer.Components4[i];

                if (ComponentManager.HasComponent<DeathTagComponent>(entityId) ||
                    ComponentManager.HasComponent<DeathAnimationComponent>(entityId))
                {
                    return;
                }

                var move = directionComponent.Direction.normalized * moveSpeedComponent.Speed * deltaTime;

                rigidbodyComponent.Rigidbody.MovePosition(rigidbodyComponent.Rigidbody.position + move);

                positionComponent.Position = rigidbodyComponent.Rigidbody.position;
            }
        }
    }
}
