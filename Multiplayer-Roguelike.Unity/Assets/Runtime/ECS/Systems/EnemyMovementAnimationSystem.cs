using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class EnemyMovementAnimationSystem : BaseSystem
    {
        public EnemyMovementAnimationSystem()
        {
            RegisterRequiredComponent(typeof(EnemyTagComponent));
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var enemyTagComponent = components[0] as EnemyTagComponent;
            var animatorComponent = components[1] as AnimatorComponent;
            var directionComponent = components[2] as DirectionComponent;
            
            animatorComponent.Animator.SetBool(animatorComponent.IsRun, directionComponent.Direction != Vector3.zero);
        }
    }
}