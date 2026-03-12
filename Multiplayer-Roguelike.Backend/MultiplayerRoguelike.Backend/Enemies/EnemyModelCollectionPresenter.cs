using Backend.Session;
using DotRecast.Core.Numerics;

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
            var startPosition = new RcVec3f(enemy.Shared.Position.Value.Xf, enemy.Shared.Position.Value.Yf, enemy.Shared.Position.Value.Zf);

            var dtCrowdAgent = _sessionModel.Navigation.Crowd.AddAgent(startPosition, _sessionModel.Navigation.Config.AgentParams);
            enemy.CrowdAgent = dtCrowdAgent;
            _sessionModel.GameSessionSharedModel.Characters.TryGet(enemy.Shared.TargetPlayerId.Value, out var character);
            _sessionModel.Navigation.SetAgentTarget(dtCrowdAgent, character.Position.Value);
        }

        private void HandleEnemyRemoved(EnemyModel enemy)
        {
            _sessionModel.Navigation.Crowd.RemoveAgent(enemy.CrowdAgent);
            enemy.CrowdAgent = null;
        }
    }
}
