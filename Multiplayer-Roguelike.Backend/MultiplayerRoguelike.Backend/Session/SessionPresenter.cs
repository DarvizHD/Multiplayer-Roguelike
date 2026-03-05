using Backend.Player;
using Shared.Models;

namespace Backend.Session
{
    public class SessionPresenter : IPresenter
    {
        private readonly SessionModel _model;

        public SessionPresenter(SessionModel model)
        {
            _model = model;
        }

        public void Enable()
        {
            _model.Players.OnAdded += OnPlayerAdded;
            _model.Players.OnRemoved += OnPlayerRemoved;
        }

        public void Disable()
        {
            _model.Players.OnAdded -= OnPlayerAdded;
            _model.Players.OnRemoved -= OnPlayerRemoved;
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
