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

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var speedComponent = components[0] as MoveSpeedComponent;
            var rotationSpeedComponent = components[1] as RotationSpeedComponent;
            var freezeMovementComponent = components[2] as FreezeMovementComponent;

            speedComponent.Speed = 0f;
            rotationSpeedComponent.Speed = 0f;

            if (freezeMovementComponent.CurrentDuration <= 0)
            {
                speedComponent.Speed = freezeMovementComponent.CachedMoveSpeedSpeed;
                rotationSpeedComponent.Speed = freezeMovementComponent.CachedRotationSpeed;
                ComponentManager.RemoveComponent<FreezeMovementComponent>(id);
            }

            freezeMovementComponent.CurrentDuration -= deltaTime;
        }
    }
}
