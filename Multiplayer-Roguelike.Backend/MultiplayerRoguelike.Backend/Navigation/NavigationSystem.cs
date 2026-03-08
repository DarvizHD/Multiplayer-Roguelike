using Backend.ServerSystems;
using Backend.Session;
using Shared.Primitives;

namespace Backend.Navigation
{
    public class NavigationSystem : RegisterServerSystem<SessionModel>
    {
        public NavigationSystem(string id) : base(id)
        {
        }

        protected override void Update(SessionModel session, float deltaTime)
        {
            session.Navigation.Crowd.Update(deltaTime, null);

            foreach (var enemy in session.Enemies.Models.Values)
            {
                var agent = session.Navigation.Crowd.GetAgent(enemy.CrowdAgent.idx);
                enemy.Shared.Position.Value = new Vector3(-agent.npos.X, agent.npos.Y, agent.npos.Z);
            }
        }
    }
}
