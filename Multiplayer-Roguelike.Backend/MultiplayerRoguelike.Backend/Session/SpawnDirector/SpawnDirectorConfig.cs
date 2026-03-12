using System.Collections.Generic;
using Shared.Primitives;

namespace Backend.Session.SpawnDirector
{
    public class SpawnDirectorConfig
    {
        public readonly List<SpawnPointModel> SpawnPoints = new()
        {
            new SpawnPointModel { Position = new Vector3(-60f, 0f, -10f), Interval = 1f },
            new SpawnPointModel { Position = new Vector3(60f, 0f, -10f), Interval = 1f },
            new SpawnPointModel { Position = new Vector3(15f, 0f, 70f), Interval = 1f },
            new SpawnPointModel { Position = new Vector3(15f, 0f, -80f), Interval = 1f },
        };
        public readonly int MaxEnemiesPerWave = 10;
        public readonly float SpawnInterval = 1f;
        public readonly float WaveMultiplier = 1.5f;
    }
}
