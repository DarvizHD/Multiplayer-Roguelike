using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle.RangeAttack
{
    public class ReloadSystem : BaseSystem
    {
        private QueryBuffer<CurrentWeaponComponent, ReloadEventComponent> _startBuffer = new();
        private QueryBuffer<CurrentWeaponComponent> _tickBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _startBuffer);
            ComponentManager.Filter.Query(ref _tickBuffer);

            for (var i = 0; i < _startBuffer.Count; i++)
            {
                var entityId = _startBuffer.EntityIds[i];
                var current = _startBuffer.Components1[i];

                if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
                {
                    continue;
                }

                if (ranged.IsReloading)
                {
                    continue;
                }

                ranged.IsReloading = true;
                ranged.ReloadTimer = ranged.ReloadTime;
                ComponentManager.RemoveComponent<ReloadEventComponent>(entityId);
            }

            for (var i = 0; i < _tickBuffer.Count; i++)
            {
                var current = _tickBuffer.Components[i];

                if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
                {
                    continue;
                }

                if (!ComponentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo))
                {
                    continue;
                }

                if (!ranged.IsReloading)
                {
                    continue;
                }

                ranged.ReloadTimer -= deltaTime;

                if (!(ranged.ReloadTimer <= 0f))
                {
                    continue;
                }

                ammo.Current = ammo.Max;
                ranged.IsReloading = false;
            }
        }
    }
}
