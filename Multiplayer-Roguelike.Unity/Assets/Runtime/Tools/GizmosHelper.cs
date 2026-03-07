using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Tools
{
    public class GizmosHelper : MonoBehaviour
    {
        private QueryBuffer<PositionComponent, RotationComponent, MeleeAttackComponent> _buffer;

        private void Start()
        {
            _buffer = new();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            DrawAttackRangeGizmos();
        }

        private void DrawAttackRangeGizmos()
        {
            var componentManager = EcsWorld.DebugInstance?.ComponentManager;

            if (componentManager == null)
            {
                return;
            }

            componentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var positionComponent = _buffer.Components1[i];
                var rotationComponent = _buffer.Components2[i];
                var meleeAttackComponent = _buffer.Components3[i];

                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawWireSphere(positionComponent.Position, meleeAttackComponent.Range);

                var attackDir = Quaternion.Euler(0, rotationComponent.Angle, 0) * Vector3.forward;
                attackDir.y = 0;
                attackDir.Normalize();

                var halfAngle = meleeAttackComponent.Angle * 0.5f;
                var leftDir = Quaternion.Euler(0, -halfAngle, 0) * attackDir;
                var rightDir = Quaternion.Euler(0, halfAngle, 0) * attackDir;

                Gizmos.color = Color.red;
                Gizmos.DrawRay(positionComponent.Position, leftDir * meleeAttackComponent.Range);
                Gizmos.DrawRay(positionComponent.Position, rightDir * meleeAttackComponent.Range);

                var steps = 20;
                var prev = positionComponent.Position + leftDir * meleeAttackComponent.Range;
                for (var k = 1; k <= steps; k++)
                {
                    var t = (float)k / steps;
                    var angle = Mathf.Lerp(-halfAngle, halfAngle, t);
                    var dir = Quaternion.Euler(0, angle, 0) * attackDir;
                    var next = positionComponent.Position + dir * meleeAttackComponent.Range;
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
            }
        }
    }
}
