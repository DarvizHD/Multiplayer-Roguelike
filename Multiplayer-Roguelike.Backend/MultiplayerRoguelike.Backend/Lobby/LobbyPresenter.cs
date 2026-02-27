using System;
using Backend.Player;

namespace Backend.Lobby
{
    public class LobbyPresenter : IPresenter
    {
        private readonly LobbyModel _model;
        private readonly WorldModel _world;

        public LobbyPresenter(LobbyModel model, WorldModel world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _model.OnMemberAdded += OnMemberAdded;
            _model.OnMemberRemoved += OnMemberRemoved;
        }

        public void Disable()
        {
            _model.OnMemberAdded -= OnMemberAdded;
            _model.OnMemberRemoved -= OnMemberRemoved;
        }

        private void OnMemberAdded(string playerNickname)
        {
            PlayerModel player = _world.Players.Get(playerNickname);
            player.PartyId = _model.Guid;

            Console.WriteLine($"Player {playerNickname} added to lobby {_model.Guid}");
        }

        private void OnMemberRemoved(string playerNickname)
        {
            PlayerModel player = _world.Players.Get(playerNickname);
            player.PartyId = string.Empty;
            
            Console.WriteLine($"Player {playerNickname} removed from lobby {_model.Guid}");
        }
    }
}
