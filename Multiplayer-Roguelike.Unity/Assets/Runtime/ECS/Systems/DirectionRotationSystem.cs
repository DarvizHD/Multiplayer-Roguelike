using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    namespace Runtime.ECS.Systems
    {
        public class DirectionRotationSystem : BaseSystem
        {
            private const float RotationSpeed = 900f;
            private const float MinAngle = 0.01f;

            public DirectionRotationSystem()
            {
                RegisterRequiredComponent(typeof(DirectionRotationTagComponent));
                RegisterRequiredComponent(typeof(TransformComponent));
                RegisterRequiredComponent(typeof(DirectionComponent));
                RegisterRequiredComponent(typeof(RotationComponent));
            }

            protected override void Update(int id, object[] components, float deltaTime)
            {
                var directionRotationTagComponent = components[0] as DirectionRotationTagComponent;
                var transformComponent = components[1] as TransformComponent;
                var directionComponent = components[2] as DirectionComponent;
                var rotationComponent =  components[3] as RotationComponent;

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
                
                var currentRotation = Quaternion.Euler(0, rotationComponent.Angle, 0f);
                
                rotationComponent.Angle =  Quaternion.RotateTowards(currentRotation, targetRotation, maxDelta).eulerAngles.y;
            }
        }
    }
}