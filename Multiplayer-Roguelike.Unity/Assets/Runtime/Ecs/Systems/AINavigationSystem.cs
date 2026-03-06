using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems
{
    public class AINavigationSystem : BaseSystem
    {
        private QueryBuffer<NavMeshAgentComponent, PositionComponent> _agentsBuffer = new();
        private QueryBuffer<PlayerTagComponent, PositionComponent> _playersBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _agentsBuffer);
            ComponentManager.Filter.Query(ref _playersBuffer);

            for (var i = 0; i < _agentsBuffer.Count; i++)
            {
                var entityId = _agentsBuffer.EntityIds[i];

                var navMeshAgentComponent = _agentsBuffer.Components1[i];
                var positionComponent = _agentsBuffer.Components2[i];

                var closestDistance = float.MaxValue;
                Vector3 closestPosition = default;

                for (var j = 0; j < _playersBuffer.Count; j++)
                {
                    var playerId = _playersBuffer.EntityIds[j];

                    var playerTagComponent = _playersBuffer.Components1[j];
                    var playerPositionComponent = _playersBuffer.Components2[j];

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
