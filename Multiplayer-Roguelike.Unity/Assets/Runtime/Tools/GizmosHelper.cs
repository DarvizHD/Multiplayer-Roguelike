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
            var results = _ecsWorld.ComponentManager.Query<PositionComponent, RotationComponent, MeleeAttackComponent>();

            foreach (var (id, positionComponent, rotationComponent, meleeAttackComponent) in results)
            {
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
                for (var i = 1; i <= steps; i++)
                {
                    var t = (float)i / steps;
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
