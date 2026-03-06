using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyMovementAnimationSystem : BaseSystem
    {
        public EnemyMovementAnimationSystem()
        {
            RegisterRequiredComponent(typeof(EnemyTagComponent));
            RegisterRequiredComponent(typeof(AnimatorComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, enemyTagComponent, animatorComponent, directionComponent)
                     in ComponentManager.Query<EnemyTagComponent, AnimatorComponent, DirectionComponent>())
            {
                animatorComponent.Animator.SetBool(animatorComponent.IsRun, directionComponent.Direction != Vector3.zero);
            }
        }
    }
}
