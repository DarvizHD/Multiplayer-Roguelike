using System;
using System.Collections.Generic;

namespace Backend.Session.Collection
{
    public class SessionModelCollectionPresenter : IPresenter
    {
        private readonly SessionModelCollection _models;
        private readonly WorldModel _worldModel;
        private readonly Dictionary<string, SessionPresenter> _presenters = new();

        public SessionModelCollectionPresenter(SessionModelCollection models, WorldModel worldModel)
        {
            _models = models;
            _worldModel = worldModel;
        }

        public void Enable()
        {
            _models.OnAdded += OnAdded;
            _models.OnRemoved += OnRemoved;
        }

        public void Disable()
        {
            foreach (var presenter in _presenters.Values)
            {
                presenter.Disable();
            }

            _presenters.Clear();

            _models.OnAdded -= OnAdded;
            _models.OnRemoved -= OnRemoved;
        }

        private void OnAdded(GameSessionModel gameSession)
        {
            var presenter = new SessionPresenter(gameSession, _worldModel);
            presenter.Enable();
            _presenters.Add(gameSession.Id, presenter);
        }

        private void OnRemoved(GameSessionModel gameSession)
        {
            Console.WriteLine($"Session {gameSession.Id} has been removed");

            var presenter = _presenters[gameSession.Id];
            presenter.Disable();
            _presenters.Remove(gameSession.Id);
        }
    }
}
