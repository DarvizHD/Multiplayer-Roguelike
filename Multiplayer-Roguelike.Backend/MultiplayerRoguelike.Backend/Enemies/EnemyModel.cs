using DotRecast.Detour.Crowd;
using Shared.Models;
using Shared.Models.Enemy;
using Shared.Primitives;

namespace Backend.Enemies
{
    public class EnemyModel
    {
        public int Id { get; }
        public EnemySharedModel Shared { get; }
        public DtCrowdAgent CrowdAgent { get; set; }
        public Vector3 LastTargetPosition { get; set; } = new(0, 0, 0);

        public EnemyModel(int id)
        {
            Id = id;
            Shared = new EnemySharedModel(id.ToString());
        }
    }
}
