using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
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
                        ComponentManager.AddComponent(entityId, new DamageParticleEventComponent());
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
