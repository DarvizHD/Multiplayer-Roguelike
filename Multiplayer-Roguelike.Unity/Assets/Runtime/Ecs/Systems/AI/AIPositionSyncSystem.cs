using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.AI
{
    public class AIPositionSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<NavMeshAgentComponent, PositionComponent> _buffer = new();

        protected override void Update(int i, float deltaTime)
        {
            var navMeshAgentComponent = _buffer.Components1[i];
            var positionComponent = _buffer.Components2[i];

            positionComponent.Position = navMeshAgentComponent.Agent.transform.position;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }
    }
}
