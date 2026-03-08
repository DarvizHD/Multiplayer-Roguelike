using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackAnimationSystem : BaseSystem
    {
        private QueryBuffer<AnimatorComponent, CurrentWeaponComponent, AttackEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var animator = _buffer.Components1[i];
                var current = _buffer.Components2[i];

                if (!ComponentManager.HasComponent<MeleeAttackComponent>(current.WeaponEntityId))
                {
                    continue;
                }

                animator.Animator.SetTrigger(animator.MeleeAttack);
            }
        }
    }
}
