using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterAttackSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<AttackEventComponent, CharacterNetworkSyncComponent, CharacterConnectionComponent, LocalControllableTag>  _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var attackEventComponent = _buffer.Components1[i];
            var characterNetworkSyncComponent = _buffer.Components2[i];
            var characterConnectionComponent = _buffer.Components3[i];

            if (!ComponentManager.TryGetComponent<EnemyNetworkSyncComponent>(attackEventComponent.TargetId, out var enemyNetworkSyncComponent))
            {
                return;
            }

            var targetId = enemyNetworkSyncComponent.EnemySharedModel.Id;

            var attackCommand = new PlayerAttackCommand(characterNetworkSyncComponent.CharacterSharedModel.Id, targetId);

            attackCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);

            ComponentManager.RemoveComponent<AttackEventComponent>(entityId);
        }
    }
}
