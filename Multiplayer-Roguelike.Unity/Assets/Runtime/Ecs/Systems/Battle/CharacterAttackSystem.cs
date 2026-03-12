using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterAttackSystem : BaseSystem
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

                var targetId = enemyNetworkSyncComponent.EnemySharedModel.Id;

                var attackCommand = new PlayerAttackCommand(characterNetworkSyncComponent.CharacterSharedModel.Id, targetId);

                attackCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);

                ComponentManager.RemoveComponent<AttackEventComponent>(entityId);
            }
        }
    }
}
