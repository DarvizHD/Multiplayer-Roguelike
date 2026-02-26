using Runtime.ECS.Components;
using Runtime.ECS.Components.Health;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class DeathSystem : BaseSystem
    {
        private const float DEATH_ANIMATION_DURATION = 2f; //TODO: возможно перенести куда-то

        public DeathSystem()
        {
            RegisterRequiredComponent(typeof(HealthComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var health = components[0] as HealthComponent;

            if (health.CurrentHealth <= 0 &&
                !ComponentManager.HasComponent<DeathComponent>(id) &&
                !ComponentManager.HasComponent<DeathAnimationComponent>(id))
            {
                ComponentManager.AddComponent(id, new DeathComponent());

                ComponentManager.AddComponent(id, new DeathAnimationComponent(DEATH_ANIMATION_DURATION));

                if (ComponentManager.TryGetComponent<AnimatorComponent>(id, out var animator))
                {
                    animator.Animator.SetTrigger("Death"); //TODO: Добавить анимацию в Unity
                }
            }
        }
    }
}
