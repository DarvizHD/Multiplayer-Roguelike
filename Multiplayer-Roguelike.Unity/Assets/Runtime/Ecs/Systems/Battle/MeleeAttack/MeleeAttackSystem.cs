using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle.MeleeAttack
{
    public class MeleeAttackSystem : BaseSystem
    {
        private QueryBuffer<PositionComponent, EnemyTagComponent>  _targetsComponentBuffer = new();
        private QueryBuffer<PositionComponent, RotationComponent, MeleeAttackComponent, AttackCooldownComponent> _attackerBuffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _targetsComponentBuffer);
            ComponentManager.Filter.Query(ref _attackerBuffer);

            for (var i = 0; i < _attackerBuffer.Count; i++)
            {
                var entityId = _attackerBuffer.EntityIds[i];
                var positionComponent = _attackerBuffer.Components1[i];
                var rotationComponent = _attackerBuffer.Components2[i];
                var meleeAttackComponent = _attackerBuffer.Components3[i];
                var attackCooldownComponent = _attackerBuffer.Components4[i];

                if (attackCooldownComponent.CurrentCooldown > 0f)
                {
                    continue;
                }

                var attackDirection = Quaternion.Euler(0f, rotationComponent.Angle, 0f) * Vector3.forward;
                attackDirection.y = 0;
                attackDirection.Normalize();

                for (var k = 0; k < _targetsComponentBuffer.Count; k++)
                {
                    var targetId = _targetsComponentBuffer.EntityIds[k];
                    var targetPositionComponent = _targetsComponentBuffer.Components1[k];

                    var toTarget = targetPositionComponent.Position - positionComponent.Position;
                    toTarget.y = 0;
                    var distance = toTarget.magnitude;

                    if (distance >= meleeAttackComponent.Range)
                    {
                        continue;
                    }

                    var angle = Vector3.Angle(attackDirection, toTarget);

                    if (angle > meleeAttackComponent.Angle * 0.5f)
                    {
                        continue;
                    }

                    ComponentManager.AddComponent(entityId, new AttackEventComponent(targetId, meleeAttackComponent.Damage));
                    attackCooldownComponent.CurrentCooldown = attackCooldownComponent.Cooldown;
                }
            }
        }
    }
}
