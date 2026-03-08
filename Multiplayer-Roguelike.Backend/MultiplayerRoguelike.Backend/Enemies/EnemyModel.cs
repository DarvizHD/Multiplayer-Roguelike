using DotRecast.Detour.Crowd;
using Shared.Models;

namespace Backend.Enemies
{
    public class EnemyModel
    {
        public int Id { get; }
        public EnemySharedModel Shared { get; }
        public DtCrowdAgent CrowdAgent { get; set; }

        public EnemyModel(int id)
        {
            Id = id;
            Shared = new EnemySharedModel(id.ToString());
        }
    }
}
