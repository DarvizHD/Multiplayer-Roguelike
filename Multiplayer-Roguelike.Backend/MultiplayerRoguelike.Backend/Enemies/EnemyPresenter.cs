using Backend.Session;

namespace Backend.Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly EnemyModel _enemyModel;
        private readonly GameSessionModel _gameSessionModel;

        public EnemyPresenter(EnemyModel enemyModel, GameSessionModel gameSessionModel)
        {
            _enemyModel = enemyModel;
            _gameSessionModel = gameSessionModel;
        }
        public void Enable()
        {
            _enemyModel.Shared.Health.OnChanged += HandleHealthChanged;
        }

        public void Disable()
        {
            _enemyModel.Shared.Health.OnChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(float health)
        {
            if (health <= 0)
            {
                _gameSessionModel.GameSessionWaveModel.IncrementEnemiesKilled();
                _gameSessionModel.Enemies.Remove(_enemyModel.Id);
            }
        }
    }
}
