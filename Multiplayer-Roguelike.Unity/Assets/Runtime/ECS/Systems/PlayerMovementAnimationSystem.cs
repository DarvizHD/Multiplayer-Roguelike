using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components
{
    public class PlayerMovementAnimationSystem : BaseSystem
    {
        public PlayerMovementAnimationSystem()
        {
            RegisterRequiredComponent(typeof(PlayerInputComponent));
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var inputComponent = components[0] as PlayerInputComponent;
            var playerAnimatorComponent = components[1] as AnimatorComponent;
            var rotationComponent = components[2] as RotationComponent;
            
            var input = inputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();
            
            var worldMove = new Vector3(input.x, 0f, input.y); 
            var rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);
            
            var localMove = Quaternion.Inverse(rotation) * worldMove;

            playerAnimatorComponent.Animator.SetFloat(playerAnimatorComponent.X, localMove.x, 0.1f, deltaTime);
            playerAnimatorComponent.Animator.SetFloat(playerAnimatorComponent.Z, localMove.z, 0.1f, deltaTime);
            playerAnimatorComponent.Animator.SetBool(playerAnimatorComponent.IsRun, input != Vector2.zero);
        }
    }
}