using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle.RangeAttack
{
    public class RangedAttackSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _attackerBuffer;
        private QueryBuffer<CurrentWeaponComponent, CursorWorldPositionComponent, WeaponSlotsComponent, LocalControllableTag> _attackerBuffer = new();
        private QueryBuffer<PositionComponent, EnemyTagComponent, AliveTagComponent> _targetsBuffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _attackerBuffer);
            ComponentManager.Filter.Query(ref _targetsBuffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _attackerBuffer.EntityIds[i];
            var current = _attackerBuffer.Components1[i];
            var cursorPos = _attackerBuffer.Components2[i];

            if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
            {
                return;
            }

            if (!ComponentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo))
            {
                return;
            }

            if (!ComponentManager.TryGetComponent<AttackCooldownComponent>(current.WeaponEntityId, out var cooldown))
            {
                return;
            }

            if (cooldown.CurrentCooldown > 0f)
            {
                return;
            }

            if (ranged.IsReloading)
            {
                return;
            }

            if (ammo.Current <= 0)
            {
                ComponentManager.AddComponent(entityId, new ReloadEventComponent());
                return;
            }

            var target = FindClosestInRadius(cursorPos.Position, ranged.AimRadius);
            if (!target.HasValue)
            {
                return;
            }

            ComponentManager.AddComponent(entityId, new ShootParticleEventComponent());
            ComponentManager.AddComponent(entityId, new AttackEventComponent(entityId, target.Value));
            ammo.Current--;
            cooldown.CurrentCooldown = cooldown.Cooldown;

            ComponentManager.AddComponent(entityId, new PlaySoundEventComponent(ranged.ShootClip));
        }

        private ushort? FindClosestInRadius(Vector3 center, float radius)
        {
            var closestDistance = float.MaxValue;
            ushort? closestTarget = null;

            for (var i = 0; i < _targetsBuffer.Count; i++)
            {
                var targetId = _targetsBuffer.EntityIds[i];
                var targetPos = _targetsBuffer.Components1[i];
                var distance = Vector3.Distance(center, targetPos.Position);

                if (distance > radius || distance >= closestDistance)
                {
                    continue;
                }

                closestDistance = distance;
                closestTarget = targetId;
            }

            return closestTarget;
        }
    }
}
