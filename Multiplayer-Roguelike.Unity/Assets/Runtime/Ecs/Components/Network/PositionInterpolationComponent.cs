using UnityEngine;

namespace Runtime.Ecs.Components.Network
{
    public class PositionInterpolationComponent : IComponent
    {
        public Vector3 TargetPosition;
        public float TargetTime = 0f;

        public Vector3 LastPosition;
        public float LastTime = 0f;

        public float TotalTime = 0f;

        public PositionInterpolationComponent(Vector3 targetPosition, Vector3 lastPosition)
        {
            TargetPosition = targetPosition;
            LastPosition = lastPosition;
        }
    }
}
