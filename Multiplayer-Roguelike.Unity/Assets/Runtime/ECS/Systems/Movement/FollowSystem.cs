using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.Movement
{
    public class FollowSystem : BaseSystem
    {
        private const float StopDistance = 1f;
        
        public FollowSystem()
        {
            RegisterRequiredComponent(typeof(FollowComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));            
            RegisterRequiredComponent(typeof(PositionComponent));            
        }
    
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var followComponent = (FollowComponent)components[0];
            var directionComponent = (DirectionComponent)components[1];
            var positionComponent = (PositionComponent)components[2];

            var toTarget = followComponent.Target.position - positionComponent.Position;
            toTarget.y = 0f;

            var distance = toTarget.magnitude;

            if (distance <= StopDistance)
            {
                directionComponent.Direction = Vector3.zero;
                return;
            }

            directionComponent.Direction = toTarget.normalized;
        }
    }
}