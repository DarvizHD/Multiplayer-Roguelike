using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyAttackAnimationSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<EnemyTagComponent, AnimatorComponent, AttackAnimationEventComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var animator = _buffer.Components2[i];

            animator.Animator.SetTrigger(animator.MeleeAttack);
            ComponentManager.RemoveComponent<AttackAnimationEventComponent>(entityId);
        }
    }
}
