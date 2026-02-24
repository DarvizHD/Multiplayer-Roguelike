using Runtime.Components;
using Runtime.Components.Movement;
using UnityEngine;

namespace Runtime.Systems
{
    public class MovementSystem : BaseSystem
    {
        public MovementSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));            
            RegisterRequiredComponent(typeof(DirectionComponent));            
            RegisterRequiredComponent(typeof(SpeedComponent));            
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent = components[0] as PositionComponent;
            var directionComponent = components[1] as DirectionComponent;
            var speedComponent = components[2] as SpeedComponent;
            
            positionComponent.Position += directionComponent.Direction.normalized * speedComponent.Speed * deltaTime;
            
            //Debug.Log($"{this.GetType()} {id}: {positionComponent.Position}");
        }
    }
}