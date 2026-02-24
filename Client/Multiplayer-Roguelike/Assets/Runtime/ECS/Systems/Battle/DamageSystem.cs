using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components.Battle
{
    public class DamageSystem : BaseSystem
    {
        public DamageSystem()
        {
            RegisterRequiredComponent(typeof(PendingDamageEventComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var pendingDamageEventComponent = components[0] as PendingDamageEventComponent;
            
            Debug.Log($"{this.GetType().Name} {id}: {pendingDamageEventComponent.TotalDamage}");
            
            ComponentManager.RemoveComponent<PendingDamageEventComponent>(id);
        }
    }
}