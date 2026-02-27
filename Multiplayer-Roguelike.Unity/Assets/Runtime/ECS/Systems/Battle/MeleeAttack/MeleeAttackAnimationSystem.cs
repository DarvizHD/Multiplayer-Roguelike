using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;

namespace Runtime.ECS.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        public MeleeAttackAnimationSystem()
        {
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(MeleeAttackComponent));
            RegisterRequiredComponent(typeof(AttackEventComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, animatorComponent, meleeAttackComponent, attackEventComponent)
                     in ComponentManager.Query<AnimatorComponent, MeleeAttackComponent, AttackEventComponent>())
            {
                animatorComponent.Animator.SetTrigger(animatorComponent.MeleeAttack);
            }
        }
    }
}
