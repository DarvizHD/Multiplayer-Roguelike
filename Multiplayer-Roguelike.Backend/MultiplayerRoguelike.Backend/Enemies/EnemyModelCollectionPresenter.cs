using Backend.ServerSystems;
using Backend.Session;
using DotRecast.Core.Numerics;
using DotRecast.Detour.Crowd;

namespace Backend.Enemies
{
    public class EnemyModelCollectionPresenter : IPresenter
    {
        private readonly EnemyModelCollection _modelCollection;
        private readonly SessionModel _sessionModel;
        private readonly ServerSystemCollection _serverSystems;
        private readonly EnemyTargetSystem _targetSystem;

        public EnemyModelCollectionPresenter(EnemyModelCollection modelCollection, SessionModel sessionModel, ServerSystemCollection serverSystems)
        {
            _modelCollection = modelCollection;
            _sessionModel = sessionModel;
            _serverSystems = serverSystems;
            _targetSystem = new EnemyTargetSystem(sessionModel.Id, sessionModel);
        }

        public void Enable()
        {
            _modelCollection.OnAdded += HandleEnemyAdded;
            _modelCollection.OnRemoved += HandleEnemyRemoved;

            _serverSystems.Add(_targetSystem);
        }

        public void Disable()
        {
            _modelCollection.OnAdded -= HandleEnemyAdded;
            _modelCollection.OnRemoved -= HandleEnemyRemoved;

            _serverSystems.Remove(_targetSystem);
        }

        private void HandleEnemyAdded(EnemyModel enemy)
        {
            var startPosition = new RcVec3f(enemy.Shared.Position.Value.Xf, enemy.Shared.Position.Value.Yf, enemy.Shared.Position.Value.Zf);

            var dtCrowdAgent = _sessionModel.Navigation.Crowd.AddAgent(startPosition, CreateAgentParams());
            enemy.CrowdAgent = dtCrowdAgent;
            _sessionModel.GameSessionSharedModel.Characters.TryGet(enemy.Shared.TargetPlayerId.Value, out var character);
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
                maxSpeed = 1f,
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
