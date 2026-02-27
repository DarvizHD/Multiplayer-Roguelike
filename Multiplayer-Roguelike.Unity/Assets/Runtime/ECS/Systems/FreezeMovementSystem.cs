using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components
{
    public class FreezeMovementSystem : BaseSystem
    {
        public FreezeMovementSystem()
        {
            RegisterRequiredComponent(typeof(MoveSpeedComponent));
            RegisterRequiredComponent(typeof(RotationSpeedComponent));
            RegisterRequiredComponent(typeof(FreezeMovementComponent));
        }

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
