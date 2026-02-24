using Runtime.ECS.Components.Movement;
using Runtime.ECS.Systems;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        public ECS.Core.ECS Ecs { get; } = new ();

        private void Start()
        {
            for (var i = 0; i < 10; i++)
            {
                Ecs.AddEntityComponent(i, new PositionComponent(Random.insideUnitSphere));
                Ecs.AddEntityComponent(i, new DirectionComponent(Random.insideUnitSphere.normalized));
                Ecs.AddEntityComponent(i, new SpeedComponent(Random.value * 5f));
                Ecs.AddEntityComponent(i, new TransformComponent(GameObject.CreatePrimitive(PrimitiveType.Sphere).transform));
            }

            Ecs.AddSystem<MovementSystem>();
            Ecs.AddSystem<PatrolSystem>();
            Ecs.AddSystem<DrawTransformSystem>();
        }

        private void Update()
        {
            Ecs.Update(Time.deltaTime);
        }
    }
}