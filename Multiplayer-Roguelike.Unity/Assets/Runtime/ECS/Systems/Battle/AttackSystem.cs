using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class AttackSystem : BaseSystem
    {
        public AttackSystem()
        {
            RegisterRequiredComponent(typeof(AttackEventComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, attackEventComponent)
                     in ComponentManager.Query<AttackEventComponent>())
            {
                Debug.Log($"System Attack {entityId} -> {attackEventComponent.TargetId} {attackEventComponent.Damage}");

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
