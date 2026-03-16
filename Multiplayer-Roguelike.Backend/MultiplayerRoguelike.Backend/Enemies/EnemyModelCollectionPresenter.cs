using System.Collections.Generic;
using Backend.Session;
using DotRecast.Core.Numerics;

namespace Backend.Enemies
{
    public class EnemyModelCollectionPresenter : IPresenter
    {
        private readonly EnemyModelCollection _modelCollection;
        private readonly GameSessionModel _gameSessionModel;
        private readonly Dictionary<int, EnemyPresenter> _enemyPresenters = new();

        public EnemyModelCollectionPresenter(EnemyModelCollection modelCollection, GameSessionModel gameSessionModel)
        {
            _modelCollection = modelCollection;
            _gameSessionModel = gameSessionModel;
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
            EnemyPresenter enemyPresenter = new(enemy, _gameSessionModel);
            enemyPresenter.Enable();
            _enemyPresenters.Add(enemy.Id, enemyPresenter);

            var startPosition = new RcVec3f(enemy.Shared.Position.Value.Xf, enemy.Shared.Position.Value.Yf, enemy.Shared.Position.Value.Zf);

            var dtCrowdAgent = _gameSessionModel.Navigation.Crowd.AddAgent(startPosition, _gameSessionModel.Navigation.Config.AgentParams);
            enemy.CrowdAgent = dtCrowdAgent;
        }

        private void HandleEnemyRemoved(EnemyModel enemy)
        {
            var enemyPresenter = _enemyPresenters[enemy.Id];
            enemyPresenter.Disable();
            _enemyPresenters.Remove(enemy.Id);

            _gameSessionModel.Navigation.Crowd.RemoveAgent(enemy.CrowdAgent);
            enemy.CrowdAgent = null;
        }
    }
}
