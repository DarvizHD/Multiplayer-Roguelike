using System.Linq;
using Backend.ServerSystems;
using Backend.Session;

namespace Backend.Enemies
{
    public class EnemyPositionSyncSystem : RegisterServerSystem<GameSessionModel>
    {
        private float _timer;
        private const float _tickInterval = 1f;

        private int _currentIndex = 0;
        private const int _batchSize = 8;

        public EnemyPositionSyncSystem(string id) : base(id)
        {
        }

        protected override void Update(GameSessionModel gameSession, float deltaTime)
        {
            _timer += deltaTime;

            if (_timer < _tickInterval)
            {
                return;
            }

            _timer = 0f;

            var enemies = gameSession.Enemies.Models.Values.ToList();
            var processed = 0;

            while (processed < _batchSize && enemies.Count > 0)
            {
                if (_currentIndex >= enemies.Count)
                {
                    _currentIndex = 0;
                }

                var enemy = enemies[_currentIndex];

                enemy.Shared.Position.Value = enemy.Position;

                _currentIndex++;
                processed++;
            }
        }
    }
}
