using System.Linq;
using Backend.Enemies;
using Backend.Player;
using Shared.Models.Player;

namespace Backend.Session
{
    public class SessionPresenter : IPresenter
    {
        private readonly SessionModel _model;
        private readonly WorldModel _world;

        private readonly EnemyModelCollectionPresenter _enemyModelCollectionPresenter;

        public SessionPresenter(SessionModel model, WorldModel worldModel)
        {
            _model = model;
            _world = worldModel;

            _enemyModelCollectionPresenter = new EnemyModelCollectionPresenter(model.Enemies, model, worldModel.ServerSystems);
        }

        public void Enable()
        {
            _model.Players.OnAdded += OnPlayerAdded;
            _model.Players.OnRemoved += OnPlayerRemoved;

            _enemyModelCollectionPresenter.Enable();
        }

        public void Disable()
        {
            _model.Players.OnAdded -= OnPlayerAdded;
            _model.Players.OnRemoved -= OnPlayerRemoved;

            _enemyModelCollectionPresenter.Disable();

            foreach (var player in _model.Players.Models.Values.Where(p => !p.IsActive))
            {
                _world.Players.Remove(player.PlayerSharedModel.Id);
            }
        }

        private void OnPlayerAdded(PlayerModel player)
        {
            player.SessionId = _model.Id;

            var character = new CharacterSharedModel(player.PlayerSharedModel.Id);
            _model.GameSessionSharedModel.Characters.Add(character);
        }

        private void OnPlayerRemoved(PlayerModel player)
        {
            player.SessionId = string.Empty;

            if (_model.GameSessionSharedModel.Characters.TryGet(player.PlayerSharedModel.Id, out var character))
            {
                _model.GameSessionSharedModel.Characters.Remove(character);
            }
        }
    }
}
