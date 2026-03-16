using System;
using System.Linq;
using Backend.ServerSystems;

namespace Backend.Session
{
    public class SessionStateSystem : IServerSystem
    {
        public string Id { get; }

        private readonly GameSessionModel _model;

        public SessionStateSystem(string id, GameSessionModel model)
        {
            Id = id;
            _model = model;
        }

        public void Update(float deltaTime)
        {
            _model.SessionTime += TimeSpan.FromSeconds(deltaTime);

            if (_model.SharedModel.SessionTime.Value != _model.SessionTime.ToString(@"mm\:ss"))
            {
                _model.SharedModel.SessionTime.Value = _model.SessionTime.ToString(@"mm\:ss");
            }

            if (_model.NeedStop)
            {
                _model.SharedModel.IsRun.Value = false;
            }

            if (_model.SharedModel.Characters.Models.All(character => character.Health.Value <= 0))
            {
                _model.NeedStop = true;
            }
        }
    }
}
