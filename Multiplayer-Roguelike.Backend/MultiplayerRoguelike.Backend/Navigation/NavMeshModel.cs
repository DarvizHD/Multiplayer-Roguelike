using DotRecast.Detour;

namespace Backend.Navigation
{
    public class NavMeshModel
    {
        public DtNavMesh NavMesh { get; set; }
        public DtNavMeshQuery Query { get; set; }

        public bool IsLoaded => NavMesh != null;

        public void SetNavMesh(DtNavMesh navMesh)
        {
            NavMesh = navMesh;
            Query = new DtNavMeshQuery(NavMesh);
        }
    }
}
