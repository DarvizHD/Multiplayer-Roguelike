using System.ComponentModel;
using Runtime.Components;
using Runtime.Components.Movement;
using Runtime.Core;
using Runtime.Systems;
using UnityEngine;

namespace Runtime
{
    public class EntryPoint : MonoBehaviour
    {
        private ECS _ecs;
        
        private void Start()
        {
            _ecs = new ECS();

            for (int i = 0; i < 10; i++)
            {
                _ecs.AddEntityComponent(i, new PositionComponent(Random.insideUnitSphere));
                _ecs.AddEntityComponent(i, new DirectionComponent(Random.insideUnitSphere.normalized));
                _ecs.AddEntityComponent(i, new SpeedComponent(Random.value * 5f));
                _ecs.AddEntityComponent(i, new TransformComponent(GameObject.CreatePrimitive(PrimitiveType.Sphere).transform));
            }
            
            _ecs.AddSystem<MovementSystem>();
            _ecs.AddSystem<PatrolSystem>();
            _ecs.AddSystem<DrawTransformSystem>();
        }

        private void Update()
        {
            _ecs.Update(Time.deltaTime);
        }
    }
}