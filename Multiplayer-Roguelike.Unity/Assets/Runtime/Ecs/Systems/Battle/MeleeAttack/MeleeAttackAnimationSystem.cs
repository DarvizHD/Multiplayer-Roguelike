using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        private QueryBuffer<AnimatorComponent, MeleeAttackComponent, AttackEventComponent> _buffer;

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var animatorComponent = _buffer.Components1[i];
                animatorComponent.Animator.SetTrigger(animatorComponent.MeleeAttack);
            }
        }
    }
}
