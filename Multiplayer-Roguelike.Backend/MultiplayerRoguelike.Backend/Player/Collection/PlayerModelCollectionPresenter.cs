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
        }

        public void Disable()
        {
            _playerModelCollection.OnAdded -= OnAdded;
        }

        private void OnAdded(PlayerModel newPlayer)
        {
            Console.WriteLine($"Player {newPlayer.PlayerNickname} has been added");
        }
    }
}