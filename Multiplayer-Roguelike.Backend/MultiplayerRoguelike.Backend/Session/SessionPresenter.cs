using System.Linq;
using Backend.Enemies;
using Backend.Player;
using Backend.ServerSystems;
using Shared.Models.Player;

namespace Backend.Session
{
    public class SessionPresenter : IPresenter
    {
        private readonly SessionModel _model;
        private readonly WorldModel _world;
        private readonly ServerSystemCollection _serverSystems;

        private readonly EnemyModelCollectionPresenter _enemyModelCollectionPresenter;
        private readonly EnemyAttackSystem _attackSystem;

        public SessionPresenter(SessionModel model, WorldModel worldModel)
        {
            _model = model;
            _world = worldModel;
            _serverSystems = worldModel.ServerSystems;

            _enemyModelCollectionPresenter = new EnemyModelCollectionPresenter(model.Enemies, model, worldModel.ServerSystems);
            _attackSystem = new EnemyAttackSystem($"{_model.Id}: enemy-attack-system", _model);
        }

        public void Enable()
        {
            _model.Players.OnAdded += OnPlayerAdded;
            _model.Players.OnRemoved += OnPlayerRemoved;

            _enemyModelCollectionPresenter.Enable();
            _serverSystems.Add(_attackSystem);
        }

        public void Disable()
        {
            _model.Players.OnAdded -= OnPlayerAdded;
            _model.Players.OnRemoved -= OnPlayerRemoved;

            _enemyModelCollectionPresenter.Disable();

            _serverSystems.Remove(_attackSystem);

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
