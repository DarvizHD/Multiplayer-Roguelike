using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;

namespace Runtime.ECS.Systems.Battle
{
    public class AttackSystem : BaseSystem
    {
        private QueryBuffer<AttackEventComponent> _attackEventBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _attackEventBuffer);

            for (var i = 0; i < _attackEventBuffer.Count; i++)
            {
                var attackEventComponent = _attackEventBuffer.Components[i];
                var entityId = _attackEventBuffer.EntityIds[i];

                if (!ComponentManager.TryGetComponent<PendingDamageEventComponent>(attackEventComponent.TargetId, out var pendingDamageEventComponent))
                {
                    ComponentManager.AddComponent(attackEventComponent.TargetId, pendingDamageEventComponent = new PendingDamageEventComponent());
                }

                pendingDamageEventComponent.TotalDamage += attackEventComponent.Damage;

                ComponentManager.RemoveComponent<AttackEventComponent>(entityId);
            }
        }
    }
}
