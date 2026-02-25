using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;

namespace Runtime.ECS.Systems
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        public MeleeAttackAnimationSystem()
        {
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(MeleeAttackComponent));
            RegisterRequiredComponent(typeof(AttackEventComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var animatorComponent = components[0] as AnimatorComponent;

            animatorComponent.Animator.SetTrigger(animatorComponent.MeleeAttack);
        }
    }
}