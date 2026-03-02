using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;

namespace Runtime.ECS.Components
{
    public class AIPositionSyncSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            var query = ComponentManager.TupleQuery<NavMeshAgentComponent, PositionComponent>();

            for (var i = 0; i < query.count; i++)
            {
                var entityId = query.entityIds[i];

                var navMeshAgentComponent = query.components1[i];
                var positionComponent = query.components2[i];

                positionComponent.Position = navMeshAgentComponent.Agent.transform.position;
            }
        }
    }
}
