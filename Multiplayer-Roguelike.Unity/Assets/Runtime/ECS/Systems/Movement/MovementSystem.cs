using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;

namespace Runtime.ECS.Systems.Movement
{
    public class MovementSystem : BaseSystem
    {
        public MovementSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
            RegisterRequiredComponent(typeof(MoveSpeedComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, positionComponent, directionComponent, moveSpeedComponent)
                     in ComponentManager.Query<PositionComponent, DirectionComponent, MoveSpeedComponent>())
            {
                if (ComponentManager.HasComponent<DeathTagComponent>(entityId) ||
                    ComponentManager.HasComponent<DeathAnimationComponent>(entityId))
                    return;

                positionComponent.Position += directionComponent.Direction.normalized * moveSpeedComponent.Speed * deltaTime;
            }
        }
    }
}
