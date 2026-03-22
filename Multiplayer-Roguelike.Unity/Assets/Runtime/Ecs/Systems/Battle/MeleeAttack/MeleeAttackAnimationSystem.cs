using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<AnimatorComponent, CurrentWeaponComponent, AttackAnimationEventComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var animator = _buffer.Components1[i];
            var current = _buffer.Components2[i];

            if (!ComponentManager.HasComponent<MeleeAttackComponent>(current.WeaponEntityId))
            {
                return;
            }

            animator.Animator.SetTrigger(animator.MeleeAttack);
            ComponentManager.RemoveComponent<AttackAnimationEventComponent>(entityId);
        }
    }
}
