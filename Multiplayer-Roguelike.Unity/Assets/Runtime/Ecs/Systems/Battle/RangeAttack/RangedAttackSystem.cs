using Runtime.ECS.Components;
using Runtime.ECS.Components.Battle;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Network;
using Runtime.Ecs.Components.Sound;
using Runtime.ECS.Components.Tags;
using Runtime.ECS.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Battle.RangeAttack
{
    public class RangedAttackSystem : BaseSystem
    {
        private QueryBuffer<CurrentWeaponComponent, CursorWorldPositionComponent, WeaponSlotsComponent, LocalControllableTag> _attackerBuffer = new();
        private QueryBuffer<PositionComponent, EnemyTagComponent, AliveTagComponent> _targetsBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _attackerBuffer);
            ComponentManager.Filter.Query(ref _targetsBuffer);

            for (var i = 0; i < _attackerBuffer.Count; i++)
            {
                var entityId = _attackerBuffer.EntityIds[i];
                var current = _attackerBuffer.Components1[i];
                var cursorPos = _attackerBuffer.Components2[i];
                var weaponSlots = _attackerBuffer.Components3[i];

                if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
                {
                    continue;
                }

                if (!ComponentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo))
                {
                    continue;
                }

                if (!ComponentManager.TryGetComponent<AttackCooldownComponent>(current.WeaponEntityId, out var cooldown))
                {
                    continue;
                }

                if (cooldown.CurrentCooldown > 0f)
                {
                    continue;
                }

                if (ranged.IsReloading)
                {
                    continue;
                }

                if (ammo.Current <= 0)
                {
                    ComponentManager.AddComponent(entityId, new ReloadEventComponent());
                    continue;
                }

                var target = FindClosestInRadius(cursorPos.Position, ranged.AimRadius);
                if (!target.HasValue)
                {
                    continue;
                }

                ComponentManager.AddComponent(entityId, new AttackEventComponent(entityId, target.Value));
                ammo.Current--;
                cooldown.CurrentCooldown = cooldown.Cooldown;

                ComponentManager.AddComponent(entityId, new PlaySoundEventComponent(ranged.ShootClip));
            }
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
