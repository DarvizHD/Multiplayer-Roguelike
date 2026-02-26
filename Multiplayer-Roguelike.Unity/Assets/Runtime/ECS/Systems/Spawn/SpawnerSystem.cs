using Runtime.ECS.Components;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Spawn;
using Runtime.ECS.Components.Tags;
using UnityEngine;

namespace Runtime.ECS.Systems.Spawn
{
    public class SpawnerSystem : BaseSystem
    {
        private int _nextEntityId = 1000; //TODO: нужен уникальный id
        private Transform _playerTransform;

        public SpawnerSystem()
        {
            RegisterRequiredComponent(typeof(SpawnerComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var spawner = components[0] as SpawnerComponent;

            if (_playerTransform == null)
            {
                FindPlayerTransform();
            }

            spawner.CurrentCount = CountAliveSpawnedUnits();

            if (spawner.CurrentCount < spawner.TargetCount)
            {
                SpawnUnit(spawner);
            }
        }

        private void FindPlayerTransform()
        {
            foreach (var entityId in ComponentManager.GetAllEntities())
            {
                if (ComponentManager.HasComponent<PlayerTagComponent>(entityId) &&
                    ComponentManager.TryGetComponent<TransformComponent>(entityId, out var transformComponent))
                {
                    _playerTransform = transformComponent.Transform;
                    break;
                }
            }
        }

        private int CountAliveSpawnedUnits()
        {
            int count = 0;
            foreach (var entityId in ComponentManager.GetAllEntities())
            {
                if (ComponentManager.HasComponent<SpawnedUnitTagComponent>(entityId))
                {
                    if (!ComponentManager.HasComponent<DeathComponent>(entityId))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void SpawnUnit(SpawnerComponent spawner)
        {
            var entityId = _nextEntityId++;

            var randomCircle = Random.insideUnitCircle * spawner.SpawnRadius;
            var spawnPosition = spawner.CenterPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

            var instance = Object.Instantiate(spawner.Prefab);
            instance.transform.position = spawnPosition;

            ComponentManager.AddComponent(entityId, new GameObjectComponent(instance));

            ComponentManager.AddComponent(entityId, new SpawnedUnitTagComponent());
            ComponentManager.AddComponent(entityId, new EnemyTagComponent());
            ComponentManager.AddComponent(entityId, new PositionComponent(spawnPosition));
            ComponentManager.AddComponent(entityId, new RotationComponent());
            ComponentManager.AddComponent(entityId, new DirectionComponent(Vector3.zero));
            ComponentManager.AddComponent(entityId, new TransformComponent(instance.transform));
            ComponentManager.AddComponent(entityId, new SpeedComponent(2f));
            ComponentManager.AddComponent(entityId, new HealthComponent(50f));
            ComponentManager.AddComponent(entityId, new FollowComponent(_playerTransform));
            ComponentManager.AddComponent(entityId, new DirectionRotationComponent(10f));
            ComponentManager.AddComponent(entityId, new SeparationComponent());
            ComponentManager.AddComponent(entityId, new RegenerationComponent(2f, 5f));

            var animator = instance.GetComponentInChildren<Animator>();
            ComponentManager.AddComponent(entityId, new AnimatorComponent(animator));
        }
    }
}
