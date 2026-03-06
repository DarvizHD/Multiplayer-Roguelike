using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyMovementAnimationSystem : BaseSystem
    {
        private QueryBuffer<EnemyTagComponent, AnimatorComponent, DirectionComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var animatorComponent = _buffer.Components2[i];
                var directionComponent = _buffer.Components3[i];
                animatorComponent.Animator.SetBool(animatorComponent.IsRun, directionComponent.Direction != Vector3.zero);
            }
        }
    }
}
