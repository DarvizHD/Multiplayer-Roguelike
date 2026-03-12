using System.Linq;
using Backend.Enemies;
using Backend.Enemies.Combat;
using Backend.Player;
using Backend.ServerSystems;
using Backend.Session.SpawnDirector;
using Shared.Models.Player;

namespace Backend.Session
{
    public class SessionPresenter : IPresenter
    {
        private readonly SessionModel _model;
        private readonly WorldModel _world;
        private readonly ServerSystemCollection _serverSystems;

        private readonly EnemyModelCollectionPresenter _enemyModelCollectionPresenter;
        private readonly EnemyTargetSystem _targetSystem;
        private readonly EnemyAttackSystem _attackSystem;
        private readonly SpawnDirectorSystem _spawnDirectorSystem;
        private readonly GameSessionWavePresenter _gameSessionWavePresenter;

        public SessionPresenter(SessionModel model, WorldModel worldModel)
        {
            _model = model;
            _world = worldModel;
            _serverSystems = worldModel.ServerSystems;

            _enemyModelCollectionPresenter = new EnemyModelCollectionPresenter(model.Enemies, model);
            _gameSessionWavePresenter = new GameSessionWavePresenter(model.GameSessionWaveModel);
            _targetSystem = new EnemyTargetSystem($"{_model.Id}: enemy-target-system", _model);
            _attackSystem = new EnemyAttackSystem($"{_model.Id}: enemy-attack-system", _model);
            _spawnDirectorSystem = new SpawnDirectorSystem($"{_model.Id}: spawn-director-system", model.SpawnDirector, _model);
        }

        public void Enable()
        {
            _model.Players.OnAdded += OnPlayerAdded;
            _model.Players.OnRemoved += OnPlayerRemoved;

            _enemyModelCollectionPresenter.Enable();
            _gameSessionWavePresenter.Enable();

            _serverSystems.Add(_targetSystem);
            _serverSystems.Add(_attackSystem);
            _serverSystems.Add(_spawnDirectorSystem);
        }

        public void Disable()
        {
            _model.Players.OnAdded -= OnPlayerAdded;
            _model.Players.OnRemoved -= OnPlayerRemoved;

            _enemyModelCollectionPresenter.Disable();
            _gameSessionWavePresenter.Disable();

            _serverSystems.Remove(_targetSystem);
            _serverSystems.Remove(_attackSystem);
            _serverSystems.Remove(_spawnDirectorSystem);

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
