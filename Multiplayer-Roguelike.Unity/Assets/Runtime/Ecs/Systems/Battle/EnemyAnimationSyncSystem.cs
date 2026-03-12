using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class EnemyAnimationSyncSystem : BaseSystem
    {
        private QueryBuffer<EnemyNetworkSyncComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
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
}
