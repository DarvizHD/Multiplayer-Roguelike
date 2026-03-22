using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle.RangeAttack
{
    public class RestoreAmmoSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CurrentWeaponComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var current = (_buffer).Components[i];

            if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
            {
                return;
            }

            if (!ranged.IsReloading)
            {
                return;
            }

            if (!ComponentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo))
            {
                return;
            }

            ranged.ReloadTimer -= deltaTime;

            if (ranged.ReloadTimer > 0f)
            {
                return;
            }

            var needed = ammo.Magazine - ammo.Current;
            var taken = Mathf.Min(needed, ammo.Reserve);

            ammo.Current += taken;
            ammo.Reserve -= taken;
            ranged.IsReloading = false;
        }
    }
}
