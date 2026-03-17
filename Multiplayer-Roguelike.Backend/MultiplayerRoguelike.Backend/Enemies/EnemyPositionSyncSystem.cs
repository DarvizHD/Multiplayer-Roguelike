using System.Linq;
using Backend.ServerSystems;
using Backend.Session;

namespace Backend.Enemies
{
    public class EnemyPositionSyncSystem : RegisterServerSystem<GameSessionModel>
    {
        private float _timer;
        private const float _tickInterval = 1f;

        public EnemyPositionSyncSystem(string id) : base(id)
        {
        }

        protected override void Update(GameSessionModel gameSession, float deltaTime)
        {
            _timer += deltaTime;

            if (_timer > _tickInterval)
            {
                _timer = 0f;

                foreach (var enemy in gameSession.Enemies.Models.Values)
                {
                    enemy.Shared.Position.Value = enemy.Position;
                }
            }
        }
    }
}
