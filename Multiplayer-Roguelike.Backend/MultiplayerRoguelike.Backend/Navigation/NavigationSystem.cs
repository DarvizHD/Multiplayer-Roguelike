using Backend.ServerSystems;
using Backend.Session;
using Shared.Primitives;

namespace Backend.Navigation
{
    public class NavigationSystem : RegisterServerSystem<GameSessionModel>
    {
        private const int _tickRate = 32;
        private const float _tickInterval = 1f / _tickRate;

        private const float _positionThreshold = 0.05f;
        private const float _positionThresholdSqr = _positionThreshold * _positionThreshold;

        private float _timer;

        public NavigationSystem(string id) : base(id)
        {
        }

        protected override void Update(GameSessionModel gameSession, float deltaTime)
        {
            _timer += deltaTime;

            if (_timer > _tickInterval)
            {
                gameSession.Navigation.Crowd.Update(_timer, null);
                _timer = 0f;

                foreach (var enemy in gameSession.Enemies.Models.Values)
                {
                    var agent = enemy.CrowdAgent;
                    var newPosition = new Vector3(-agent.npos.X, agent.npos.Y, agent.npos.Z);
                    var oldPosition = enemy.Position;

                    if ((newPosition - oldPosition).LengthSquared() > _positionThresholdSqr)
                    {
                        enemy.Position = newPosition;
                    }
                }
            }
        }
    }
}
