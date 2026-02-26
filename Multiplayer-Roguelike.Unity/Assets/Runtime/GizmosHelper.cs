using Runtime.ECS.Components.Battle;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Core;
using UnityEngine;

namespace Runtime
{
    public class GizmosHelper : MonoBehaviour
    {
        private ECSWorld _ecsWorld;

        public void Initialize(ECSWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || _ecsWorld == null)
            {
                return;
            }

            DrawAttackRangeGizmos();
        }

        private void DrawAttackRangeGizmos()
        {
            var results = _ecsWorld.ComponentManager.Query(
                typeof(PositionComponent),
                typeof(RotationComponent),
                typeof(MeleeAttackComponent)
            );

            foreach (var (id, components) in results)
            {
                var position = (PositionComponent)components[0];
                var rotation = (RotationComponent)components[1];
                var melee = (MeleeAttackComponent)components[2];

                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawWireSphere(position.Position, melee.Range);

                var attackDir = Quaternion.Euler(0, rotation.Angle, 0) * Vector3.forward;
                attackDir.y = 0;
                attackDir.Normalize();

                var halfAngle = melee.Angle * 0.5f;
                var leftDir = Quaternion.Euler(0, -halfAngle, 0) * attackDir;
                var rightDir = Quaternion.Euler(0, halfAngle, 0) * attackDir;

                Gizmos.color = Color.red;
                Gizmos.DrawRay(position.Position, leftDir * melee.Range);
                Gizmos.DrawRay(position.Position, rightDir * melee.Range);

                var steps = 20;
                var prev = position.Position + leftDir * melee.Range;
                for (var i = 1; i <= steps; i++)
                {
                    var t = (float)i / steps;
                    var angle = Mathf.Lerp(-halfAngle, halfAngle, t);
                    var dir = Quaternion.Euler(0, angle, 0) * attackDir;
                    var next = position.Position + dir * melee.Range;
                    Gizmos.DrawLine(prev, next);
                    prev = next;
                }
            }
        }
    }
}
