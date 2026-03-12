using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyAttackAnimationSystem : BaseSystem
    {
        private QueryBuffer<EnemyTagComponent, AnimatorComponent, AttackAnimationEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var animator = _buffer.Components2[i];

                animator.Animator.SetTrigger(animator.MeleeAttack);
                ComponentManager.RemoveComponent<AttackAnimationEventComponent>(entityId);
            }
        }
    }
}
