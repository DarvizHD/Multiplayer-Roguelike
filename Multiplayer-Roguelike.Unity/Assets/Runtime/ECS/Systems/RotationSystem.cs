using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    namespace Runtime.ECS.Systems
    {
        public class RotationSystem : BaseSystem
        {
            private const float RotationSpeed = 900f;
            private const float MinAngle = 0.01f;

            public RotationSystem()
            {
                RegisterRequiredComponent(typeof(RotationComponent));
                RegisterRequiredComponent(typeof(TransformComponent));
                RegisterRequiredComponent(typeof(DirectionComponent));
            }

            protected override void Update(int id, object[] components, float deltaTime)
            {
                var rotationComponent = components[0] as RotationComponent;
                var transformComponent = components[1] as TransformComponent;
                var directionComponent = components[2] as DirectionComponent;

                var dir = directionComponent!.Direction;
                
                if (dir == Vector3.zero)
                {
                    return;
                }

                var angle = Vector3.Angle(transformComponent!.Transform.forward, dir);
                
                if (angle < MinAngle)
                {
                    return;
                }
                
                var targetRotation = Quaternion.LookRotation(dir);
                var maxDelta = RotationSpeed * deltaTime;
                rotationComponent!.Rotation =  Quaternion.RotateTowards(transformComponent.Transform.rotation, targetRotation, maxDelta);
            }
        }
    }
}