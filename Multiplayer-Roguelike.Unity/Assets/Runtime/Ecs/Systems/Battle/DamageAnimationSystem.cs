using Runtime.Ecs.Components;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class DamageAnimationSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<AnimatorComponent, DamageAnimationEventComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var animatorComponent = _buffer.Components1[i];

            animatorComponent.Animator.SetTrigger(animatorComponent.Damage);

            ComponentManager.RemoveComponent<DamageAnimationEventComponent>(entityId);
        }
    }
}
