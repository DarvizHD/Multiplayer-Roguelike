using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Tools;

namespace Runtime.Ecs.Systems.Network
{
    public class EnemyPositionSyncSystem : BaseSystem
    {
        private QueryBuffer<EnemyNetworkSyncComponent, PositionComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterSharedModelComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];
                positionComponent.Position = characterSharedModelComponent.EnemySharedModel.Position.Value.ToUnityVector3();
            }
        }
    }
}
