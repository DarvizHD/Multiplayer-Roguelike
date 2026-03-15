using DotRecast.Detour;
using DotRecast.Detour.Crowd;

namespace Backend.Navigation
{
    public class NavigationModel
    {
        public DtNavMesh NavMesh { get; }
        public DtNavMeshQuery Query { get; }
        public DtCrowd Crowd { get; }
        public NavigationConfig Config { get; }

        public NavigationModel(DtNavMesh navMesh)
        {
            NavMesh = navMesh;
            Config = new NavigationConfig();
            Query = new DtNavMeshQuery(navMesh);
            Crowd = new DtCrowd(Config.CrowdConfig, NavMesh);
        }

        public void SetupObstacleAvoidance()
        {
            for (var i = 0; i < 4; i++)
            {
                Crowd.SetObstacleAvoidanceParams(i, Config.ObstacleParams);
            }
        }
    }
}
