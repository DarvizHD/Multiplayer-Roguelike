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
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var inputComponent = components[0] as PlayerInputComponent;
            var playerAnimatorComponent = components[1] as AnimatorComponent;

            var move = inputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();

            var isRun = move != Vector2.zero;
            
            playerAnimatorComponent.Animator.SetBool(playerAnimatorComponent.IsRun, isRun);
        }
    }
}