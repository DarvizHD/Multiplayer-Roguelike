using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Shared.Commands;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class PlayerAttackSystem : BaseSystem
    {
        private QueryBuffer<AttackEventComponent, CharacterNetworkSyncComponent, CharacterConnectionComponent, LocalControllableTag>  _attackEventBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _attackEventBuffer);

            for (var i = 0; i < _attackEventBuffer.Count; i++)
            {
                var entityId = _attackEventBuffer.EntityIds[i];
                var attackEventComponent = _attackEventBuffer.Components1[i];
                var characterNetworkSyncComponent = _attackEventBuffer.Components2[i];
                var characterConnectionComponent = _attackEventBuffer.Components3[i];

                if (!ComponentManager.TryGetComponent<EnemyNetworkSyncComponent>(attackEventComponent.TargetId, out var enemyNetworkSyncComponent))
                {
                    continue;
                }

                if (!ComponentManager.TryGetComponent<PendingDamageEventComponent>(attackEventComponent.TargetId, out var pendingDamageEventComponent))
                {
                    ComponentManager.AddComponent(attackEventComponent.TargetId, pendingDamageEventComponent = new PendingDamageEventComponent());
                }

                var targetId = enemyNetworkSyncComponent.EnemySharedModel.Id;

                Debug.Log($"Player {characterNetworkSyncComponent.CharacterSharedModel.Id} Attack {targetId}");

                var attackCommand = new PlayerAttackCommand(characterNetworkSyncComponent.CharacterSharedModel.Id, targetId, $"test");

                attackCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);

                ComponentManager.RemoveComponent<AttackEventComponent>(entityId);
            }
        }
    }
}
