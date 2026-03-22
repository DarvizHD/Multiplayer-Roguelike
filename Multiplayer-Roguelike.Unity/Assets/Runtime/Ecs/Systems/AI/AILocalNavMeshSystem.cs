using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.AI
{
    public class AILocalNavMeshSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _aiBuffer;
        private QueryBuffer<NavMeshAgentComponent, AliveTagComponent> _aiBuffer = new();
        private QueryBuffer<PositionComponent, PlayerTagComponent, AliveTagComponent> _positionBuffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _aiBuffer);
            ComponentManager.Filter.Query(ref _positionBuffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var navMeshAgentComponent = _aiBuffer.Components1[i];

            var nearestPosition = _positionBuffer.Components1[0].Position;
            var minDistance = (_positionBuffer.Components1[0].Position - navMeshAgentComponent.Agent.transform.position).sqrMagnitude;
            for (var j = 1; j < _positionBuffer.Count; j++)
            {
                var distance = (_positionBuffer.Components1[j].Position - navMeshAgentComponent.Agent.transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPosition = _positionBuffer.Components1[j].Position;
                }
            }

            navMeshAgentComponent.Agent.SetDestination(nearestPosition);
        }
    }
}
