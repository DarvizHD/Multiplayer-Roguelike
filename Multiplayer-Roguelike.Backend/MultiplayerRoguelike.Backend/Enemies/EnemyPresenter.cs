using Backend.Session;

namespace Backend.Enemies
{
    public class EnemyPresenter : IPresenter
    {
        private readonly EnemyModel _enemyModel;
        private readonly SessionModel _sessionModel;

        public EnemyPresenter(EnemyModel enemyModel, SessionModel sessionModel)
        {
            _enemyModel = enemyModel;
            _sessionModel = sessionModel;
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
                _sessionModel.GameSessionWaveModel.IncrementEnemiesKilled();
            }
        }
    }
}
