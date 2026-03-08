using Backend.Session;
using DotRecast.Core.Numerics;
using DotRecast.Detour.Crowd;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyModelCollectionPresenter : IPresenter
    {
        private readonly EnemyModelCollection _modelCollection;
        private readonly SessionModel _sessionModel;

        public EnemyModelCollectionPresenter(EnemyModelCollection modelCollection, SessionModel sessionModel)
        {
            _modelCollection = modelCollection;
            _sessionModel = sessionModel;
        }

        public void Enable()
        {
            _modelCollection.OnAdded += HandleEnemyAdded;
            _modelCollection.OnRemoved += HandleEnemyRemoved;
        }

        public void Disable()
        {
            _modelCollection.OnAdded -= HandleEnemyAdded;
            _modelCollection.OnRemoved -= HandleEnemyRemoved;
        }

        private void HandleEnemyAdded(EnemyModel enemy)
        {
            var startPosition = new RcVec3f(enemy.Shared.Position.Value.X, enemy.Shared.Position.Value.Y, enemy.Shared.Position.Value.Z);

            var dtCrowdAgent = _sessionModel.Navigation.Crowd.AddAgent(startPosition, CreateAgentParams());
            enemy.CrowdAgent = dtCrowdAgent;
            _sessionModel.GameSessionSharedModel.Characters.TryGet(enemy.TargetPlayerId, out var character);
            _sessionModel.Navigation.SetAgentTarget(dtCrowdAgent, character.Position.Value);
        }

        private void HandleEnemyRemoved(EnemyModel enemy)
        {
            _sessionModel.Navigation.Crowd.RemoveAgent(enemy.CrowdAgent);
            enemy.CrowdAgent = null;
        }

        private DtCrowdAgentParams CreateAgentParams()
        {
            return new DtCrowdAgentParams
            {
                radius = 0.6f,
                height = 2.0f,
                maxAcceleration = 8.0f,
                maxSpeed = 3.5f,
                collisionQueryRange = 2.0f,
                pathOptimizationRange = 1.5f,
                separationWeight = 2.0f,
                updateFlags = 0,
                obstacleAvoidanceType = 3,
                queryFilterType = 0,
                userData = null
            };
        }
    }
}
