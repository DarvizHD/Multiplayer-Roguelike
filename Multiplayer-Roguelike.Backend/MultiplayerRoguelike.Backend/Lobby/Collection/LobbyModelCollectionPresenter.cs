using System;
using System.Collections.Generic;

namespace Backend.Lobby.Collection
{
    public class LobbyModelCollectionPresenter : IPresenter
    {
        private readonly LobbyModelCollection _lobbyModelCollection;

        private readonly Dictionary<string, LobbyPresenter> _presenters = new();
        private readonly WorldModel _world;

        public LobbyModelCollectionPresenter(LobbyModelCollection lobbyModelCollection, WorldModel world)
        {
            _lobbyModelCollection = lobbyModelCollection;
            _world = world;
        }

        public void Enable()
        {
            _lobbyModelCollection.OnAdded += OnLobbyAdded;
            _lobbyModelCollection.OnRemoved += OnLobbyRemoved;
        }

        public void Disable()
        {
            _lobbyModelCollection.OnAdded -= OnLobbyAdded;
            _lobbyModelCollection.OnRemoved -= OnLobbyRemoved;

            foreach (KeyValuePair<string, LobbyPresenter> presenter in _presenters)
            {
                presenter.Value.Disable();
            }

            _presenters.Clear();
        }

        private void OnLobbyAdded(LobbyModel lobbyModel)
        {
            Console.WriteLine($"Lobby {lobbyModel.Guid} created");

            var presenter = new LobbyPresenter(lobbyModel, _world);
            presenter.Enable();

            _presenters.Add(lobbyModel.Guid, presenter);
        }

        private void OnLobbyRemoved(LobbyModel lobbyModel)
        {
            LobbyPresenter presenter = _presenters[lobbyModel.Guid];
            presenter.Disable();

            _presenters.Remove(lobbyModel.Guid);
        }
    }
}
