using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Movement.Freeze;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.Movement.Freeze
{
    public class FreezeMovementByDamageSystem : BaseSystem
    {
        private QueryBuffer<MoveSpeedComponent, FreezeMovementByDamageComponent, RotationSpeedComponent,
            PendingDamageEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var moveSpeedComponent = _buffer.Components1[i];
                var freezeMovementByDamageComponent = _buffer.Components2[i];
                var rotationSpeedComponent = _buffer.Components3[i];

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
