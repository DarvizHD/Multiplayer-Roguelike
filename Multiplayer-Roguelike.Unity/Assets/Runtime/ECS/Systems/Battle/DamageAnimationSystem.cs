using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;

namespace Runtime.ECS.Systems.Battle
{
    public class DamageAnimationSystem : BaseSystem
    {
        public DamageAnimationSystem()
        {
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(PendingDamageEventComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var animatorComponent =  components[0] as AnimatorComponent;
            var pendingDamageEventComponent = components[1] as PendingDamageEventComponent;

            animatorComponent.Animator.SetTrigger(animatorComponent.DamageTrigger);
        }
    }
}
