using Runtime.Ecs.Components;
using Runtime.ECS.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class DamageAnimationSystem : BaseSystem
    {
        private QueryBuffer<AnimatorComponent, DamageAnimationEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var animatorComponent = _buffer.Components1[i];

                animatorComponent.Animator.SetTrigger(animatorComponent.Damage);

                ComponentManager.RemoveComponent<DamageAnimationEventComponent>(entityId);
            }
        }
    }
}
