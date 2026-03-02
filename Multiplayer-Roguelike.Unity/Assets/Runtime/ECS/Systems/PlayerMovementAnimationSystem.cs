using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Components.Tags;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerMovementAnimationSystem : BaseSystem
    {
        public PlayerMovementAnimationSystem()
        {
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, directionComponent, playerTagComponent, animatorComponent, rotationComponent)
                     in ComponentManager.Query<DirectionComponent, PlayerTagComponent, AnimatorComponent, RotationComponent>())
            {
                var worldMove = directionComponent.Direction;
                var rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);

                var localMove = Quaternion.Inverse(rotation) * worldMove;

                animatorComponent.Animator.SetFloat(animatorComponent.X, localMove.x, 0.1f, deltaTime);
                animatorComponent.Animator.SetFloat(animatorComponent.Z, localMove.z, 0.1f, deltaTime);
            }
        }
    }
}
