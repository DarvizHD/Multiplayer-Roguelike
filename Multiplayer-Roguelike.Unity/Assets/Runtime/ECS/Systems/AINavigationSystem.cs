using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Tags;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime.ECS.Components
{
    public class AINavigationSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            var agents = ComponentManager.TupleQuery<NavMeshAgentComponent, PositionComponent>();
            var players = ComponentManager.TupleQuery<PlayerTagComponent, PositionComponent>();

            for (var i = 0; i < agents.count; i++)
            {
                var entityId = agents.entityIds[i];

                var navMeshAgentComponent = agents.components1[i];
                var positionComponent = agents.components2[i];

                var closestDistance = float.MaxValue;
                Vector3 closestPosition = default;

                for (var j = 0; j < players.count; j++)
                {
                    var playerId = players.entityIds[j];

                    var playerTagComponent = players.components1[j];
                    var playerPositionComponent = players.components2[j];

                    var distance = Vector3.Distance(positionComponent.Position, playerPositionComponent.Position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPosition = playerPositionComponent.Position;
                    }

                    if (navMeshAgentComponent == null)
                    {
                        Debug.Log($"{entityId}: {navMeshAgentComponent == null}");
                    }
                }

                navMeshAgentComponent.Agent.SetDestination(closestPosition);
            }
        }
    }
}
