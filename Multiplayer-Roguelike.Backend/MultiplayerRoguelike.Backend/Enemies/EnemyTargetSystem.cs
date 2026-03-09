using Backend.Navigation;
using Backend.ServerSystems;
using Backend.Session;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;
using Shared.Models;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyTargetSystem : IServerSystem
    {
        public string Id { get; }

        private readonly SessionModel _sessionModel;

        public EnemyTargetSystem(string id, SessionModel sessionModel)
        {
            _sessionModel = sessionModel;
            Id = id;
        }

        public void Update(float deltaTime)
        {
            foreach (var enemy in _sessionModel.Enemies.Models.Values)
            {
                var player = SelectTargetPlayer(_sessionModel, enemy);

                enemy.Shared.TargetPlayerId.Value = player.Id;

                var targetPosition = player.Position.Value;

                SetAgentTarget(_sessionModel.Navigation, enemy.CrowdAgent, targetPosition);
            }
        }

        private void SetAgentTarget(NavigationModel navigation, DtCrowdAgent agent, Vector3 targetPosition)
        {
            var halfExtents = new RcVec3f(1, 2, 1);
            navigation.Query.FindNearestPoly(
                new RcVec3f(-targetPosition.Xf, targetPosition.Yf, targetPosition.Zf),
                halfExtents,
                new DtQueryDefaultFilter(),
                out var nearestRef,
                out var nearestPt,
                out _
            );

            agent.SetTarget(nearestRef, nearestPt);
        }

        private CharacterSharedModel SelectTargetPlayer(SessionModel session, EnemyModel enemy)
        {
            CharacterSharedModel closestCharacter = null;
            var closestDistance = float.MaxValue;

            foreach (var character in session.GameSessionSharedModel.Characters.Models)
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
