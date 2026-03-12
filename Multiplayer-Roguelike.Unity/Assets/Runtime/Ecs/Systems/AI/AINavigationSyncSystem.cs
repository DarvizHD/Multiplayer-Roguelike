using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Tools;
using Shared.Models.Player;
using UnityEngine;

namespace Runtime.Ecs.Systems.AI
{
    public class AINavigationSyncSystem : BaseSystem
    {
        private const float _softThreshold = 0.25f;
        private const float _hardThreshold = 4f;

        private QueryBuffer<EnemyNetworkSyncComponent, NavMeshAgentComponent, AliveTagComponent> _buffer = new();
        private QueryBuffer<CharacterNetworkSyncComponent> _playersBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);
            ComponentManager.Filter.Query(ref _playersBuffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var enemySharedModel = _buffer.Components1[i];
                var navMeshAgentComponent = _buffer.Components2[i];

                var serverPosition = enemySharedModel.EnemySharedModel.Position.Value.ToUnityVector3();

                var delta = (navMeshAgentComponent.Agent.nextPosition - serverPosition).sqrMagnitude;

                if (delta is > _softThreshold and < _hardThreshold)
                {
                    navMeshAgentComponent.Agent.Warp(Vector3.Lerp(navMeshAgentComponent.Agent.transform.position, serverPosition, 0.1f));
                }
                else if (delta >= _hardThreshold)
                {
                    navMeshAgentComponent.Agent.Warp(serverPosition);
                }

                var targetId = enemySharedModel.EnemySharedModel.TargetPlayerId.Value;

                if (targetId == null)
                {
                    continue;
                }

                CharacterSharedModel founded = null;

                for (var k = 0; k < _buffer.Count; k++)
                {
                    var characterNetworkSyncComponent = _playersBuffer.Components[k];

                    if (characterNetworkSyncComponent.CharacterSharedModel.Id == targetId)
                    {
                        founded = characterNetworkSyncComponent.CharacterSharedModel;
                        break;
                    }
                }

                if (founded != null)
                {
                    navMeshAgentComponent.Agent.SetDestination(founded.Position.Value.ToUnityVector3());
                }
            }
        }
    }
}
