using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class DamageAnimationSystem : BaseSystem
    {
        private QueryBuffer<AnimatorComponent, PendingDamageEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var animatorComponent = _buffer.Components1[i];

                animatorComponent.Animator.SetTrigger(animatorComponent.Damage);
            }
        }
    }
}
