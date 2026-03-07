using Backend.Navigation;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyModel
    {
        public int Id { get; }

        public Vector3 Position;

        public string TargetPlayerId;

        public NavAgentModel Agent;

        public EnemyModel(int id)
        {
            Id = id;
        }
    }
}
