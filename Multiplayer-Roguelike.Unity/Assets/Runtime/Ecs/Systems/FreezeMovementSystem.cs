using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Movement.Freeze;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems
{
    public class FreezeMovementSystem : BaseSystem
    {
        private QueryBuffer<MoveSpeedComponent, RotationSpeedComponent, FreezeMovementComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var moveSpeedComponent = _buffer.Components1[i];
                var rotationSpeedComponent = _buffer.Components2[i];
                var freezeMovementComponent = _buffer.Components3[i];
                var entityId = _buffer.EntityIds[i];

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
