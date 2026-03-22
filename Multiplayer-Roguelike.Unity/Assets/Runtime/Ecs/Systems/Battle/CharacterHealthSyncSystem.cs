using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterHealthSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<HealthComponent, CharacterNetworkSyncComponent, AliveTagComponent, PlayerTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var healthComponent = _buffer.Components1[i];
            var characterNetworkSyncComponent = _buffer.Components2[i];

            if (characterNetworkSyncComponent.CharacterSharedModel.Health.IsDirty)
            {
                if (healthComponent.CurrentHealth > characterNetworkSyncComponent.CharacterSharedModel.Health.Value)
                {
                    ComponentManager.AddComponent(entityId, new DamageAnimationEventComponent());
                    ComponentManager.AddComponent(entityId, new DeathParticleEventComponent());
                }

                healthComponent.CurrentHealth = characterNetworkSyncComponent.CharacterSharedModel.Health.Value;
            }

            if (healthComponent.CurrentHealth <= 0f)
            {
                ComponentManager.AddComponent(entityId, new DeathEventComponent());
            }

            characterNetworkSyncComponent.CharacterSharedModel.Health.ClearDirty();
        }
    }
}
