using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.Movement
{
    public class PatrolSystem : BaseSystem
    {
        public PatrolSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(DirectionComponent));
            RegisterRequiredComponent(typeof(SpeedComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent =  components[0] as PositionComponent;
            var directionComponent = components[1] as DirectionComponent;
            var speedComponent = components[2] as SpeedComponent;
            
            var endPoint = directionComponent.Direction.normalized * speedComponent.Speed;
            
            if (Vector3.Distance(positionComponent.Position, endPoint) < 0.5f)
            {
                directionComponent.Direction *= -1f;
            }
        }
    }
}