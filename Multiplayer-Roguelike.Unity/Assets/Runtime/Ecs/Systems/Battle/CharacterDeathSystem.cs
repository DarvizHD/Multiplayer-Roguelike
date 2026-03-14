using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterDeathSystem : BaseSystem
    {
        private QueryBuffer<DeathEventComponent, AnimatorComponent, PlayerTagComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var animatorComponent = _buffer.Components2[i];

                ComponentManager.RemoveComponent<AliveTagComponent>(entityId);
                ComponentManager.RemoveComponent<DeathEventComponent>(entityId);
                ComponentManager.AddComponent<DeathParticleEventComponent>(entityId, new DeathParticleEventComponent());
                ComponentManager.AddComponent(entityId, new DeathTagComponent());

                animatorComponent.Animator.SetTrigger(animatorComponent.Death);
            }
        }
    }
}
