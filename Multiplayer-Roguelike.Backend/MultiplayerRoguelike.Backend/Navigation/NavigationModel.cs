using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;
using Shared.Primitives;

namespace Backend.Navigation
{
    public class NavigationModel
    {
        public DtNavMesh NavMesh { get; }
        public DtNavMeshQuery Query { get; }

        public DtCrowd Crowd { get; }

        public NavigationModel(DtNavMesh navMesh)
        {
            NavMesh = navMesh;
            Query = new DtNavMeshQuery(navMesh);
            var config = new DtCrowdConfig(0.6f);
            Crowd = new DtCrowd(config, NavMesh);
        }

        public void SetupObstacleAvoidance()
        {
            var obstacleAvoidanceParams = new DtObstacleAvoidanceParams();

            for (var i = 0; i < 4; i++)
            {
                Crowd.SetObstacleAvoidanceParams(i, obstacleAvoidanceParams);
            }
        }

        public void SetAgentTarget(DtCrowdAgent agent, Vector3 target)
        {
            var targetPos = new RcVec3f(target.Xf, target.Yf, target.Zf);

            var halfExtents = new RcVec3f(2, 4, 2);

            var filter = new DtQueryDefaultFilter();

            var result = Query.FindNearestPoly(targetPos, halfExtents, filter, out var polyRef, out var polyPos, out _);

            if (result.Succeeded())
            {
                agent.SetTarget(polyRef, polyPos);
            }
        }
    }
}
