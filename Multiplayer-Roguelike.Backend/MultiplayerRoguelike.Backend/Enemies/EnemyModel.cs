using Backend.Navigation;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyModel
    {
        public int Id { get; }
        public NavAgentModel Agent { get; }

        public Vector3 Position;

        public string TargetPlayerId;

        public EnemyModel(int id, NavAgentModel agent)
        {
            Id = id;
            Agent = agent;
        }
    }
}
