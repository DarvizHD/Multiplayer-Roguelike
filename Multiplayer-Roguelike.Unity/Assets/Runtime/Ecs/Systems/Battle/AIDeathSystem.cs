using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;

namespace Runtime.ECS.Systems.Battle
{
    public class AIDeathSystem : BaseSystem
    {
        private QueryBuffer<DeathEventComponent, AnimatorComponent, RagdollComponent, NavMeshAgentComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var animatorComponent = _buffer.Components2[i];
                var ragdollComponent = _buffer.Components3[i];
                var navMeshAgentComponent = _buffer.Components4[i];

                ComponentManager.RemoveComponent<DeathEventComponent>(entityId);

                ragdollComponent.RagdollProvider.Enable = true;
                animatorComponent.Animator.enabled = false;
                navMeshAgentComponent.Agent.enabled = false;
            }
        }
    }
}
