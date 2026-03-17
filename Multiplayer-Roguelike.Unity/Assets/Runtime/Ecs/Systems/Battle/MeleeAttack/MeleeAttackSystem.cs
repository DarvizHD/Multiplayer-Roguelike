using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _attackerBuffer;

        private QueryBuffer<PositionComponent, RotationComponent, CurrentWeaponComponent, LocalControllableTag> _attackerBuffer = new();
        private QueryBuffer<PositionComponent, EnemyTagComponent, AliveTagComponent> _targetsBuffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _targetsBuffer);
            ComponentManager.Filter.Query(ref _attackerBuffer);
        }

        protected override void Update(int i, float deltaTime)
        {
                var entityId = _attackerBuffer.EntityIds[i];
                var position = _attackerBuffer.Components1[i];
                var rotation = _attackerBuffer.Components2[i];
                var current = _attackerBuffer.Components3[i];

                if (!ComponentManager.TryGetComponent<MeleeAttackComponent>(current.WeaponEntityId, out var melee))
                {
                    return;
                }

                if (!ComponentManager.TryGetComponent<AttackCooldownComponent>(current.WeaponEntityId,
                        out var cooldown))
                {
                    return;
                }

                if (cooldown.CurrentCooldown > 0f)
                {
                    return;
                }

                var attackDir = Quaternion.Euler(0f, rotation.Angle, 0f) * Vector3.forward;
                attackDir.y = 0;
                attackDir.Normalize();

                for (var k = 0; k < _targetsBuffer.Count; k++)
                {
                    var targetId = _targetsBuffer.EntityIds[k];
                    var targetPos = _targetsBuffer.Components1[k];

                    var toTarget = targetPos.Position - position.Position;
                    toTarget.y = 0;
                    var distance = toTarget.magnitude;

                    if (distance >= melee.Range)
                    {
                        continue;
                    }

                    var angle = Vector3.Angle(attackDir, toTarget);
                    if (angle > melee.Angle * 0.5f)
                    {
                        continue;
                    }

                    ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                    ComponentManager.AddComponent(entityId, new AttackEventComponent(entityId, targetId));
                    cooldown.CurrentCooldown = cooldown.Cooldown;

                    ComponentManager.AddComponent(entityId, new PlaySoundEventComponent(melee.AttackClip));
                }
        }
    }
}
