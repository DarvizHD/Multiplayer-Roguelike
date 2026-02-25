using Runtime.ECS.Components.Movement;
using UnityEngine;

namespace Runtime.ECS.Systems.Movement
{
    public class DrawTransformSystem : BaseSystem
    {
        public DrawTransformSystem()
        {
            RegisterRequiredComponent(typeof(PositionComponent));
            RegisterRequiredComponent(typeof(RotationComponent));
            RegisterRequiredComponent(typeof(TransformComponent));
        }
        
        protected override void Update(int id, object[] components, float deltaTime)
        {
            var positionComponent =  components[0] as PositionComponent;
            var rotationComponent = components[1] as RotationComponent;
            var transformComponent = components[2] as TransformComponent;
            
            transformComponent.Transform.position = positionComponent.Position;
            transformComponent.Transform.rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);
        }
    }
}