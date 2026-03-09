using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems
{
    public class WeaponAnimationSwitcherSystem : BaseSystem
    {
        private QueryBuffer<SwitchWeaponEventComponent, AnimatorComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var switchEventComponent = _buffer.Components1[i];
                var animatorComponent = _buffer.Components2[i];

                animatorComponent.Animator.SetInteger(animatorComponent.WeaponId, switchEventComponent.TargetSlot);
            }
        }
    }
}
