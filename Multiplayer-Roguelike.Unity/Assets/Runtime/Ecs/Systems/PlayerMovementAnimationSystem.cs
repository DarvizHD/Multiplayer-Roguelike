using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems
{
    public class PlayerMovementAnimationSystem : BaseSystem
    {
        private QueryBuffer<DirectionComponent, AnimatorComponent, RotationComponent, PlayerTagComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var directionComponent = _buffer.Components1[i];
                var animatorComponent = _buffer.Components2[i];
                var rotationComponent = _buffer.Components3[i];

                var worldMove = directionComponent.Direction;
                var rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);

                var localMove = Quaternion.Inverse(rotation) * worldMove;

                animatorComponent.Animator.SetFloat(animatorComponent.X, localMove.x, 0.1f, deltaTime);
                animatorComponent.Animator.SetFloat(animatorComponent.Z, localMove.z, 0.1f, deltaTime);
            }
        }
    }
}
