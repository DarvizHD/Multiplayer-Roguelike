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
        private const float _targetMoveThreshold = 0.5f;
        private const float _targetMoveThresholdSqr = _targetMoveThreshold * _targetMoveThreshold;

        private const float _targetUpdateInterval = 0.25f;
        private float _timer;

        public string Id { get; }

        private readonly SessionModel _sessionModel;
        private readonly IDtQueryFilter _filter = new DtQueryDefaultFilter();
        private static readonly RcVec3f _halfExtents = new(1, 2, 1);

        public EnemyTargetSystem(string id, SessionModel sessionModel)
        {
            _sessionModel = sessionModel;
            Id = id;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            if (!(_timer < _targetUpdateInterval))
            {
                _timer = 0f;

                foreach (var enemy in _sessionModel.Enemies.Models.Values)
                {
                    var player = SelectTargetPlayer(_sessionModel, enemy);

                    if (player != null)
                    {
                        var targetPosition = player.Position.Value;

                        if ((targetPosition - enemy.LastTargetPosition).LengthSquared() > _targetMoveThresholdSqr)
                        {
                            enemy.Shared.TargetPlayerId.Value = player.Id;
                            enemy.LastTargetPosition = targetPosition;

                            SetAgentTarget(_sessionModel.Navigation, enemy.CrowdAgent, targetPosition);
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

        private CharacterSharedModel SelectTargetPlayer(SessionModel session, EnemyModel enemy)
        {
            CharacterSharedModel closestCharacter = null;
            var closestDistance = float.MaxValue;

            foreach (var character in session.GameSessionSharedModel.Characters.Models.Where(c => c.Health.Value > 0))
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
