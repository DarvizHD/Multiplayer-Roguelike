using Runtime.Components.Movement;
using Runtime.Systems;
using UnityEngine;

namespace Runtime.Components
{
    public class DrawTransformSystem : BaseSystem
    {
        public DrawTransformSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(TransformComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent =  components[0] as PositionComponent;
            var transformComponent = components[1] as TransformComponent;
            
            transformComponent.Transform.position = positionComponent.Position;
        }
    }

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
            
            Debug.Log($"{this.GetType()}: {id}: {endPoint} : {directionComponent.Direction} : {speedComponent.Speed} : {positionComponent.Position}");
            
            if (Vector3.Distance(positionComponent.Position, endPoint) < 0.5f)
            {
                directionComponent.Direction *= -1f;
            }
        }
    }
}