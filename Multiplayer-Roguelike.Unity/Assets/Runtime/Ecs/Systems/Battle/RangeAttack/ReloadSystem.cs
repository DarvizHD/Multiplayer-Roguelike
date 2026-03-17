using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle.RangeAttack
{
    public class ReloadSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CurrentWeaponComponent, ReloadEventComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var current = _buffer.Components1[i];

            if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
            {
                return;
            }

            if (ranged.IsReloading)
            {
                return;
            }

            if (!ComponentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo))
            {
                return;
            }

            if (ammo.Reserve <= 0)
            {
                return;
            }

            ranged.IsReloading = true;
            ranged.ReloadTimer = ranged.ReloadTime;

            ComponentManager.RemoveComponent<ReloadEventComponent>(entityId);
            ComponentManager.AddComponent(entityId, new PlaySoundEventComponent(ranged.ReloadClip));
        }
    }
}
