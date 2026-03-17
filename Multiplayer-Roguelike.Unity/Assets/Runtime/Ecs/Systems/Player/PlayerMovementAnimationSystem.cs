using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Player
{
    public class PlayerMovementAnimationSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<DirectionComponent, AnimatorComponent, RotationComponent, PlayerTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
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
