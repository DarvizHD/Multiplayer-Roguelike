using System;

namespace Backend.Session.SpawnDirector
{
    public class GameSessionWaveModel
    {
        public int CurrentWave = 1;
        public int EnemiesSpawned = 0;
        public int EnemiesKilled = 0;
        public int EnemiesTarget = 10;
        public bool WaveActive = true;
        public readonly float BaseEnemiesPerWave = 10f;
        public readonly float WaveMultiplier = 1.5f;

        public Action OnEnemiesKilledChanged;

        public void IncrementEnemiesKilled()
        {
            EnemiesKilled++;
            OnEnemiesKilledChanged?.Invoke();
        }
    }
}
