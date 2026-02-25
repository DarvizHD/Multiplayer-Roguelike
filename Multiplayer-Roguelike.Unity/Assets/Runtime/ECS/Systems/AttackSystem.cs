using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class AttackSystem : BaseSystem
    {
        public AttackSystem()
        {
            RegisterRequiredComponent(typeof(AttackEventComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var attackEventComponent = components[0] as AttackEventComponent;
            
            Debug.Log($"System Attack {id} -> {attackEventComponent.TargetId} {attackEventComponent.Damage}");
            
            if (!ComponentManager.TryGetComponent<PendingDamageEventComponent>(attackEventComponent.TargetId, out var pendingDamageEventComponent))
            {
                ComponentManager.AddComponent(attackEventComponent.TargetId, pendingDamageEventComponent = new PendingDamageEventComponent());
            }
                
            pendingDamageEventComponent.TotalDamage += attackEventComponent.Damage;
            
            ComponentManager.RemoveComponent<AttackEventComponent>(id);
        }
    }
}