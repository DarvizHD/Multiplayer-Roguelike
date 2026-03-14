using System.Linq;
using Backend.ServerSystems;

namespace Backend.Session
{
    public class SessionStateSystem : IServerSystem
    {
        public string Id { get; }

        private readonly SessionModel _model;

        public SessionStateSystem(string id, SessionModel model)
        {
            Id = id;
            _model = model;
        }

        public void Update(float deltaTime)
        {
            if (_model.NeedStop)
            {
                _model.GameSessionSharedModel.IsRun.Value = false;
            }

            if (_model.GameSessionSharedModel.Characters.Models.All(character => character.Health.Value <= 0))
            {
                _model.NeedStop = true;
            }
        }
    }
}
