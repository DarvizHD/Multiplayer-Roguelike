using System.Collections.Generic;
using Backend.Session;
using DotRecast.Core.Numerics;

namespace Backend.Enemies
{
    public class EnemyModelCollectionPresenter : IPresenter
    {
        private readonly EnemyModelCollection _modelCollection;
        private readonly SessionModel _sessionModel;
        private readonly Dictionary<int, EnemyPresenter> _enemyPresenters = new();

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
            EnemyPresenter enemyPresenter = new(enemy, _sessionModel);
            enemyPresenter.Enable();
            _enemyPresenters.Add(enemy.Id, enemyPresenter);

            var startPosition = new RcVec3f(enemy.Shared.Position.Value.Xf, enemy.Shared.Position.Value.Yf, enemy.Shared.Position.Value.Zf);

            var dtCrowdAgent = _sessionModel.Navigation.Crowd.AddAgent(startPosition, _sessionModel.Navigation.Config.AgentParams);
            enemy.CrowdAgent = dtCrowdAgent;
        }

        private void HandleEnemyRemoved(EnemyModel enemy)
        {
            var enemyPresenter = _enemyPresenters[enemy.Id];
            enemyPresenter.Disable();
            _enemyPresenters.Remove(enemy.Id);

            _sessionModel.Navigation.Crowd.RemoveAgent(enemy.CrowdAgent);
            enemy.CrowdAgent = null;
        }
    }
}
