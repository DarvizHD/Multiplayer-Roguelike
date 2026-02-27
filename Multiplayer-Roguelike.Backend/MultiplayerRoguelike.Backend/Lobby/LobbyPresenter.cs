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

        private void OnMemberAdded(string newPlayerNickname)
        {
            PlayerModel newPlayer = _world.Players.Get(newPlayerNickname);
            newPlayer.PlayerSharedModel.Lobby.LobbyId.Value = _model.Guid;
            newPlayer.PlayerSharedModel.Lobby.OwnerId.Value = _model.OwnerNickname;
            foreach (var memberNickname in _model.Members)
            {
                newPlayer.PlayerSharedModel.Lobby.Members.Add(memberNickname);

                if (memberNickname != newPlayerNickname)
                {
                    PlayerModel member = _world.Players.Get(memberNickname);
                    member.PlayerSharedModel.Lobby.Members.Add(newPlayerNickname);
                }
            }
        }

        private void OnMemberRemoved(string removedPlayerNickname)
        {
            PlayerModel removedPlayer = _world.Players.Get(removedPlayerNickname);
            removedPlayer.PlayerSharedModel.Lobby.LobbyId.Value = string.Empty;
            removedPlayer.PlayerSharedModel.Lobby.OwnerId.Value = string.Empty;
            removedPlayer.PlayerSharedModel.Lobby.Members.Clear();

            foreach (var memberNickname in _model.Members)
            {
                PlayerModel member = _world.Players.Get(memberNickname);
                member.PlayerSharedModel.Lobby.Members.Remove(removedPlayerNickname);
            }
        }
    }
}
