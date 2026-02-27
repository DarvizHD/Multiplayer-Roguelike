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

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var speedComponent = components[0] as MoveSpeedComponent;
            var freezeMovementDamageComponent = components[1] as FreezeMovementByDamageComponent;
            var rotationSpeedComponent = components[2] as RotationSpeedComponent;

            if (ComponentManager.HasComponent<FreezeMovementComponent>(id))
            {
                return;
            }

            ComponentManager.AddComponent(id, new FreezeMovementComponent(speedComponent.Speed, rotationSpeedComponent.Speed,freezeMovementDamageComponent.Duration));
        }
    }
}
