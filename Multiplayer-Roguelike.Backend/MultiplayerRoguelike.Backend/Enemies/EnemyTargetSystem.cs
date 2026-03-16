using System.Linq;
using Backend.Navigation;
using Backend.ServerSystems;
using Backend.Session;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;
using Shared.Models.Player;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyTargetSystem : IServerSystem
    {
        private const int _tickRate = 32;
        private const float _tickInterval = 1f / _tickRate;

        private const float _targetMoveThreshold = 0.5f;
        private const float _targetMoveThresholdSqr = _targetMoveThreshold * _targetMoveThreshold;
        private float _timer;

        public string Id { get; }

        private readonly GameSessionModel _gameSessionModel;
        private readonly IDtQueryFilter _filter = new DtQueryDefaultFilter();
        private static readonly RcVec3f _halfExtents = new(1, 2, 1);

        public EnemyTargetSystem(string id, GameSessionModel gameSessionModel)
        {
            _gameSessionModel = gameSessionModel;
            Id = id;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer > _tickInterval)
            {
                _timer = 0f;

                foreach (var enemy in _gameSessionModel.Enemies.Models.Values)
                {
                    var player = SelectTargetPlayer(_gameSessionModel, enemy);

                    if (player != null)
                    {
                        var targetPosition = player.Position.Value;

                        if ((targetPosition - enemy.LastTargetPosition).LengthSquared() > _targetMoveThresholdSqr)
                        {
                            enemy.Shared.TargetPlayerId.Value = player.Id;
                            enemy.LastTargetPosition = targetPosition;

                            SetAgentTarget(_gameSessionModel.Navigation, enemy.CrowdAgent, targetPosition);
                        }
                    }
                }
            }
        }

        private void SetAgentTarget(NavigationModel navigation, DtCrowdAgent agent, Vector3 targetPosition)
        {
            navigation.Query.FindNearestPoly(
                new RcVec3f(-targetPosition.Xf, targetPosition.Yf, targetPosition.Zf),
                _halfExtents,
                _filter,
                out var nearestRef,
                out var nearestPt,
                out _
            );

            if (nearestRef != 0)
            {
                agent.SetTarget(nearestRef, nearestPt);
            }
        }

        private CharacterSharedModel SelectTargetPlayer(GameSessionModel gameSession, EnemyModel enemy)
        {
            CharacterSharedModel closestCharacter = null;
            var closestDistance = float.MaxValue;

            foreach (var character in gameSession.SharedModel.Characters.Models.Where(c => c.Health.Value > 0))
            {
                var distance = (enemy.Shared.Position.Value - character.Position.Value).LengthSquared();

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCharacter = character;
                }
            }

            return closestCharacter;
        }
    }
}
