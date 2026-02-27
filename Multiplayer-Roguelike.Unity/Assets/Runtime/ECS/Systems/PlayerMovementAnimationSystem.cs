using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerMovementAnimationSystem : BaseSystem
    {
        public PlayerMovementAnimationSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent));
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, playerInputComponent, animatorComponent, rotationComponent)
                     in ComponentManager.Query<PlayerInputComponent, AnimatorComponent, RotationComponent>())
            {
                var input = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>().normalized;

                var worldMove = new Vector3(input.x, 0f, input.y);
                var rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);

                var localMove = Quaternion.Inverse(rotation) * worldMove;

                animatorComponent.Animator.SetFloat(animatorComponent.X, localMove.x, 0.1f, deltaTime);
                animatorComponent.Animator.SetFloat(animatorComponent.Z, localMove.z, 0.1f, deltaTime);
            }
        }
    }
}
