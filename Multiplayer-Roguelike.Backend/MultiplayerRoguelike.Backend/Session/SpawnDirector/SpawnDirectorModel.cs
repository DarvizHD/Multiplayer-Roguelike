using System.Collections.Generic;

namespace Backend.Session.SpawnDirector
{
    public class SpawnDirectorModel
    {
        public readonly List<SpawnPointModel> SpawnPoints;
        public int MaxEnemiesPerWave;
        public readonly float SpawnInterval;
        public float WaveMultiplier;

        public SpawnDirectorModel(SpawnDirectorConfig config)
        {
            SpawnPoints = config.SpawnPoints;
            MaxEnemiesPerWave = config.MaxEnemiesPerWave;
            SpawnInterval = config.SpawnInterval;
            WaveMultiplier = config.WaveMultiplier;
        }
    }
}
