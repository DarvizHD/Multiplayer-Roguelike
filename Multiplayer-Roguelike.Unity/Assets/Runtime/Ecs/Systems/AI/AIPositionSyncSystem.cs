using Runtime.ECS.Components;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.AI
{
    public class AIPositionSyncSystem : BaseSystem
    {
        private QueryBuffer<NavMeshAgentComponent, PositionComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];

                var navMeshAgentComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];

                positionComponent.Position = navMeshAgentComponent.Agent.transform.position;
            }
        }
    }
}
