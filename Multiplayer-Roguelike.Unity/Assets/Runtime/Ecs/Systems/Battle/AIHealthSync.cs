using Runtime.ECS.Components;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Components.Tags;
using Runtime.ECS.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle
{
    public class AIHealthSync : BaseSystem
    {
        private QueryBuffer<HealthComponent, EnemyNetworkSyncComponent, AliveTagComponent, EnemyTagComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var healthComponent = _buffer.Components1[i];
                var enemyNetworkSyncComponent = _buffer.Components2[i];

                var hasDifferent = !Mathf.Approximately(healthComponent.CurrentHealth, enemyNetworkSyncComponent.EnemySharedModel.Health.Value);

                if (hasDifferent)
                {
                    if (healthComponent.CurrentHealth > enemyNetworkSyncComponent.EnemySharedModel.Health.Value)
                    {
                        ComponentManager.AddComponent(entityId, new DamageAnimationEventComponent());
                    }

                    healthComponent.CurrentHealth = enemyNetworkSyncComponent.EnemySharedModel.Health.Value;
                }

                if (healthComponent.CurrentHealth <= 0f)
                {
                    ComponentManager.AddComponent(entityId, new DeathEventComponent());
                }
            }
        }
    }
}
