using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
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
