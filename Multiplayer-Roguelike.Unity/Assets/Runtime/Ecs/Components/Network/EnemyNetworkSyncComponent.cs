using Shared.Models.Enemy;

namespace Runtime.Ecs.Components.Network
{
    public class EnemyNetworkSyncComponent : IComponent
    {
        public readonly EnemySharedModel EnemySharedModel;

        public EnemyNetworkSyncComponent(EnemySharedModel enemySharedModel)
        {
            EnemySharedModel = enemySharedModel;
        }
    }
}
