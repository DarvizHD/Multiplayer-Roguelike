using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Movement.Freeze;

namespace Runtime.Ecs.Systems
{
    public class FreezeMovementSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, moveSpeedComponent, rotationSpeedComponent, freezeMovementComponent)
                     in ComponentManager.Query<MoveSpeedComponent, RotationSpeedComponent, FreezeMovementComponent>())
            {
                moveSpeedComponent.Speed = 0f;
                rotationSpeedComponent.Speed = 0f;

                if (freezeMovementComponent.CurrentDuration <= 0)
                {
                    moveSpeedComponent.Speed = freezeMovementComponent.CachedMoveSpeedSpeed;
                    rotationSpeedComponent.Speed = freezeMovementComponent.CachedRotationSpeed;
                    ComponentManager.RemoveComponent<FreezeMovementComponent>(entityId);
                }

                freezeMovementComponent.CurrentDuration -= deltaTime;
            }
        }
    }
}
