using System;

namespace Backend.Player.Collection
{
    public class PlayerModelCollectionPresenter : IPresenter
    {
        private readonly PlayerModelCollection _playerModelCollection;

        public PlayerModelCollectionPresenter(PlayerModelCollection playerModelCollection)
        {
            _playerModelCollection = playerModelCollection;
        }

        public void Enable()
        {
            _playerModelCollection.OnAdded += OnAdded;
            _playerModelCollection.OnRemoved += OnRemoved;
        }

        public void Disable()
        {
            _playerModelCollection.OnAdded -= OnAdded;
            _playerModelCollection.OnRemoved -= OnRemoved;
        }

        private void OnAdded(PlayerModel newPlayer)
        {
            Console.WriteLine($"Player {newPlayer.PlayerSharedModel.Nickname.Value} has been added");
        }

        private void OnRemoved(PlayerModel oldPlayer)
        {
            Console.WriteLine($"Player {oldPlayer.PlayerSharedModel.Nickname.Value} has been removed");
        }
    }
}
