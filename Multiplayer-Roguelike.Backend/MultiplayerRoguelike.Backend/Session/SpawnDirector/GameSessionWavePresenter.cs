using System;

namespace Backend.Session.SpawnDirector
{
    public class GameSessionWavePresenter : IPresenter
    {
        private readonly GameSessionWaveModel _waveModel;
        private readonly GameSessionModel _gameSessionModel;
        public GameSessionWavePresenter(GameSessionWaveModel waveModel, GameSessionModel gameSessionModel)
        {
            _waveModel = waveModel;
            _gameSessionModel = gameSessionModel;
        }

        public void Enable()
        {
            _waveModel.OnEnemiesKilledChanged += HandleEnemiesKilledChanged;
        }

        public void Disable()
        {
            _waveModel.OnEnemiesKilledChanged -= HandleEnemiesKilledChanged;
        }

        private void HandleEnemiesKilledChanged()
        {
            if (_waveModel.EnemiesKilled >= _waveModel.EnemiesTarget)
            {
                _waveModel.CurrentWave++;
                _gameSessionModel.SharedModel.WaveNumber.Value = _waveModel.CurrentWave;
                _waveModel.EnemiesSpawned = 0;
                _waveModel.EnemiesKilled = 0;
                _waveModel.EnemiesTarget = (int)MathF.Round(_waveModel.BaseEnemiesPerWave * MathF.Pow(_waveModel.WaveMultiplier, _waveModel.CurrentWave - 1));
                _waveModel.WaveActive = true;
                Console.WriteLine($"Wave {_waveModel.CurrentWave} started");
            }
        }
    }
}
