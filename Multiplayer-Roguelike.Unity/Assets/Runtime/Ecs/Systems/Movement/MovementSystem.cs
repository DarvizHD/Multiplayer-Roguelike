using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;

namespace Runtime.Ecs.Systems.Movement
{
    public class MovementSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, positionComponent, directionComponent, moveSpeedComponent, _)
                     in ComponentManager.Query<PositionComponent, DirectionComponent, MoveSpeedComponent, LocalControllableTag>())
            {
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
