using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class DamageSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PendingDamageEventComponent, HealthComponent, AliveTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var healthComponent = _buffer.Components2[i];
            var pendingDamageEventComponent = _buffer.Components1[i];

            if (healthComponent.CurrentHealth <= 0 || ComponentManager.HasComponent<InvulnerabilityComponent>(entityId))
            {
                return;
            }

            healthComponent.CurrentHealth -= pendingDamageEventComponent.TotalDamage;

            if (ComponentManager.TryGetComponent<HitSoundComponent>(entityId, out var hitSound) && healthComponent.CurrentHealth >= 0)
            {
                ComponentManager.AddComponent(entityId, new PlaySoundEventComponent(hitSound.Clip));
            }

            if (healthComponent.CurrentHealth <= 0)
            {
                ComponentManager.AddComponent(entityId, new DeathEventComponent());
                ComponentManager.RemoveComponent<AliveTagComponent>(entityId);
            }

            if (ComponentManager.TryGetComponent<RegenerationComponent>(entityId, out var regenerationComponent))
            {
                regenerationComponent.LastDamageTime = 0f;
            }

            ComponentManager.RemoveComponent<PendingDamageEventComponent>(entityId);
        }
    }
}
