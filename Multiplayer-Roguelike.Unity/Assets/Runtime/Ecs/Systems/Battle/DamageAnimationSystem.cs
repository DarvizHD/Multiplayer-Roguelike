using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;

namespace Runtime.Ecs.Systems.Battle
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
