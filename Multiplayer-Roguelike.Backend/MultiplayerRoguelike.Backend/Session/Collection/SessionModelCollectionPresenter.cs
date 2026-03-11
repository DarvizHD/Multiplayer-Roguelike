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

        private void OnAdded(SessionModel session)
        {
            var presenter = new SessionPresenter(session, _worldModel);
            presenter.Enable();
            _presenters.Add(session.Id, presenter);
        }

        private void OnRemoved(SessionModel session)
        {
            Console.WriteLine($"Session {session.Id} has been removed");

            var presenter = _presenters[session.Id];
            presenter.Disable();
            _presenters.Remove(session.Id);
        }
    }
}
