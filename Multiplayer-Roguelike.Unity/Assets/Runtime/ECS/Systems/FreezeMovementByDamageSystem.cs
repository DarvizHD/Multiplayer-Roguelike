using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components
{
    public class FreezeMovementByDamageSystem : BaseSystem
    {
        public FreezeMovementByDamageSystem()
        {
            RegisterRequiredComponent(typeof(MoveSpeedComponent));
            RegisterRequiredComponent(typeof(FreezeMovementByDamageComponent));
            RegisterRequiredComponent(typeof(RotationSpeedComponent));
            RegisterRequiredComponent(typeof(PendingDamageEventComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, moveSpeedComponent, freezeMovementByDamageComponent, rotationSpeedComponent, pendingDamageEventComponent)
                     in ComponentManager.Query<MoveSpeedComponent, FreezeMovementByDamageComponent, RotationSpeedComponent, PendingDamageEventComponent>())
            {
                if (ComponentManager.HasComponent<FreezeMovementComponent>(entityId))
                {
                    return;
                }

                ComponentManager.AddComponent(entityId,
                    new FreezeMovementComponent(moveSpeedComponent.Speed,
                        rotationSpeedComponent.Speed,
                        freezeMovementByDamageComponent.Duration));
            }
        }
    }
}
