using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Tools
{
    public class GizmosHelper : MonoBehaviour
    {
        private QueryBuffer<PositionComponent, RotationComponent, CurrentWeaponComponent> _attackerBuffer;
        private QueryBuffer<CursorWorldPositionComponent> _cursorBuffer;

        private void Start()
        {
            _attackerBuffer = new QueryBuffer<PositionComponent, RotationComponent, CurrentWeaponComponent>();
            _cursorBuffer = new QueryBuffer<CursorWorldPositionComponent>();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var componentManager = EcsWorld.DebugInstance?.ComponentManager;
            if (componentManager == null)
            {
                return;
            }

            componentManager.Filter.Query(ref _attackerBuffer);
            componentManager.Filter.Query(ref _cursorBuffer);

            for (var i = 0; i < _attackerBuffer.Count; i++)
            {
                var position = _attackerBuffer.Components1[i].Position;
                var rotation = _attackerBuffer.Components2[i];
                var current = _attackerBuffer.Components3[i];

                if (componentManager.TryGetComponent<MeleeAttackComponent>(current.WeaponEntityId, out var melee))
                {
                    componentManager.TryGetComponent<AttackCooldownComponent>(current.WeaponEntityId, out var cooldown);
                    DrawMeleeGizmos(position, rotation.Angle, melee, cooldown);
                }
                else if (componentManager.TryGetComponent<RangedWeaponComponent>(current.WeaponEntityId, out var ranged))
                {
                    componentManager.TryGetComponent<AmmoComponent>(current.WeaponEntityId, out var ammo);
                    componentManager.TryGetComponent<AttackCooldownComponent>(current.WeaponEntityId, out var cooldown);
                    DrawRangedGizmos(position, ranged, ammo, cooldown);
                }
            }
        }

        private void DrawMeleeGizmos(Vector3 position, float rotationAngle, MeleeAttackComponent melee, AttackCooldownComponent cooldown)
        {
            var ready = cooldown == null || cooldown.CurrentCooldown <= 0f;
            var color = ready ? Color.red : Color.gray;

            var attackDir = Quaternion.Euler(0, rotationAngle, 0) * Vector3.forward;
            attackDir.y = 0;
            attackDir.Normalize();

            var halfAngle = melee.Angle * 0.5f;
            var leftDir = Quaternion.Euler(0, -halfAngle, 0) * attackDir;
            var rightDir = Quaternion.Euler(0, halfAngle, 0) * attackDir;

            Gizmos.color = new Color(color.r, color.g, color.b, 0.15f);
            Gizmos.DrawWireSphere(position, melee.Range);

            Gizmos.color = color;
            Gizmos.DrawRay(position, leftDir * melee.Range);
            Gizmos.DrawRay(position, rightDir * melee.Range);

            var steps = 20;

            var prev = position + leftDir * melee.Range;
            for (var k = 1; k <= steps; k++)
            {
                var t = (float)k / steps;
                var angle = Mathf.Lerp(-halfAngle, halfAngle, t);
                var dir = Quaternion.Euler(0, angle, 0) * attackDir;
                var next = position + dir * melee.Range;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }

#if UNITY_EDITOR
            var label = ready ? "READY" : $"CD {cooldown.CurrentCooldown:F1}s";
            UnityEditor.Handles.Label(position + Vector3.up * 2f, label);
#endif
        }

        private void DrawRangedGizmos(Vector3 position, RangedWeaponComponent ranged, AmmoComponent ammo, AttackCooldownComponent cooldown)
        {
            for (var i = 0; i < _cursorBuffer.Count; i++)
            {
                var cursorPos = _cursorBuffer.Components[i].Position;

                var ready = (cooldown == null || cooldown.CurrentCooldown <= 0f) && !ranged.IsReloading;
                var color = ready ? new Color(0f, 0.5f, 1f) : Color.gray;

                Gizmos.color = new Color(color.r, color.g, color.b, 0.15f);
                Gizmos.DrawWireSphere(cursorPos, ranged.AimRadius);

                Gizmos.color = new Color(color.r, color.g, color.b, 0.5f);
                Gizmos.DrawLine(position, cursorPos);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(cursorPos, 0.3f);

#if UNITY_EDITOR
                string label;
                if (ranged.IsReloading)
                {
                    label = $"RELOADING {ranged.ReloadTimer:F1}s";
                }
                else if (cooldown is { CurrentCooldown: > 0f })
                {
                    label = $"CD {cooldown.CurrentCooldown:F1}s";
                }
                else
                {
                    label = ammo != null ? $"{ammo.Current}/{ammo.Max}" : "READY";
                }

                UnityEditor.Handles.Label(position + Vector3.up * 2f, label);
#endif
            }
        }
    }
}
