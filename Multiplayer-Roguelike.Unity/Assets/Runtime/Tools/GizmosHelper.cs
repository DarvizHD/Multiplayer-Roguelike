using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Tools
{
    public class GizmosHelper : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private QueryBuffer<PositionComponent, RotationComponent, MeleeAttackComponent> _buffer;

        public void Initialize(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || _ecsWorld == null)
            {
                return;
            }
        }
    }
}
