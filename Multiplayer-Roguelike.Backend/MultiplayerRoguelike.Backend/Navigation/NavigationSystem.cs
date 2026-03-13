using Backend.ServerSystems;
using Backend.Session;
using Shared.Primitives;

namespace Backend.Navigation
{
    public class NavigationSystem : RegisterServerSystem<SessionModel>
    {
        private const float _positionThreshold = 0.05f;
        private const float _positionThresholdSqr = _positionThreshold * _positionThreshold;

        public NavigationSystem(string id) : base(id)
        {
        }

        protected override void Update(SessionModel session, float deltaTime)
        {
            session.Navigation.Crowd.Update(deltaTime, null);

            foreach (var enemy in session.Enemies.Models.Values)
            {
                var agent = enemy.CrowdAgent;
                var newPosition = new Vector3(-agent.npos.X, agent.npos.Y, agent.npos.Z);
                var oldPosition = enemy.Shared.Position.Value;

                if ((newPosition - oldPosition).LengthSquared() > _positionThresholdSqr)
                {
                    enemy.Shared.Position.Value = newPosition;
                }
            }
        }
    }
}
