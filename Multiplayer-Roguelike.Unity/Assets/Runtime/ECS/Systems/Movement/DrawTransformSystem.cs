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

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, positionComponent, rotationComponent, transformComponent)
                     in ComponentManager.Query<PositionComponent, RotationComponent, TransformComponent>())
            {
                transformComponent.Transform.position = positionComponent.Position;
                transformComponent.Transform.rotation = Quaternion.Euler(0f, rotationComponent.Angle, 0f);
            }
        }
    }
}
