using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;

namespace Runtime.Ecs.Systems.Battle
{
    public class AttackSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, attackEventComponent)
                     in ComponentManager.Query<AttackEventComponent>())
            {
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
