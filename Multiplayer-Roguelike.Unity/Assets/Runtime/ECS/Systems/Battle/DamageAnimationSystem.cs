using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class DamageAnimationSystem : BaseSystem
    {
        public DamageAnimationSystem()
        {
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(PendingDamageEventComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, animatorComponent, pendingDamageEventComponent)
                     in ComponentManager.Query<AnimatorComponent, PendingDamageEventComponent>())
            {
                animatorComponent.Animator.SetTrigger(animatorComponent.Damage);
            }
        }
    }
}
