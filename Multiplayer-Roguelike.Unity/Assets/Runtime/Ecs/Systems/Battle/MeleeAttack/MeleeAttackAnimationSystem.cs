using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        private QueryBuffer<AnimatorComponent, CurrentWeaponComponent, AttackAnimationEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var animator = _buffer.Components1[i];
                var current = _buffer.Components2[i];

                if (!ComponentManager.HasComponent<MeleeAttackComponent>(current.WeaponEntityId))
                {
                    continue;
                }

                animator.Animator.SetTrigger(animator.MeleeAttack);
                ComponentManager.RemoveComponent<AttackAnimationEventComponent>(entityId);
            }
        }
    }
}
