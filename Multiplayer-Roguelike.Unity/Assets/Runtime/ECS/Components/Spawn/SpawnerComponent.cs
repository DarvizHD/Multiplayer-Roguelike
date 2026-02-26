using UnityEngine;

namespace Runtime.ECS.Components.Spawn
{
    public class SpawnerComponent : IComponent
    {
        public int TargetCount;
        public int CurrentCount;
        public GameObject Prefab;
        public float SpawnRadius;
        public Vector3 CenterPosition;

        public SpawnerComponent(int targetCount, GameObject prefab, Vector3 centerPosition, float spawnRadius = 5f)
        {
            TargetCount = targetCount;
            Prefab = prefab;
            CenterPosition = centerPosition;
            SpawnRadius = spawnRadius;
            CurrentCount = 0;
        }
    }
}
