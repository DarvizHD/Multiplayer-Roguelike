using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class AIDeathSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<DeathEventComponent, AnimatorComponent, RagdollComponent, NavMeshAgentComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var animatorComponent = _buffer.Components2[i];
            var ragdollComponent = _buffer.Components3[i];
            var navMeshAgentComponent = _buffer.Components4[i];

            ComponentManager.RemoveComponent<AliveTagComponent>(entityId);
            ComponentManager.RemoveComponent<DeathEventComponent>(entityId);
            ComponentManager.AddComponent<DeathParticleEventComponent>(entityId, new DeathParticleEventComponent());
            ComponentManager.AddComponent(entityId, new DeathTagComponent());

            ragdollComponent.RagdollProvider.Enable = true;
            animatorComponent.Animator.enabled = false;
            navMeshAgentComponent.Agent.enabled = false;
        }
    }
}
