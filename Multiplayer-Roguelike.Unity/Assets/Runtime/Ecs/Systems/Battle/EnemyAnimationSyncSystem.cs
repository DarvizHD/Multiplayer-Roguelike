using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyAnimationSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<EnemyNetworkSyncComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var enemyNetworkSyncComponent = _buffer.Components[i];

            if (enemyNetworkSyncComponent.EnemySharedModel.AnimationState.IsDirty)
            {
                ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                enemyNetworkSyncComponent.EnemySharedModel.AnimationState.ClearDirty();
            }
        }
    }
}
