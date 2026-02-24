using Runtime.ECS.Components.Movement;

namespace Runtime.ECS.Systems
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
}