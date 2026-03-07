using System.IO;
using Backend.ServerSystems;
using Backend.Session;
using DotRecast.Detour;
using DotRecast.Detour.Io;

namespace Backend.Navigation
{
    public class NavigationPresenter : IPresenter
    {
        private const string _filename = "base.navmesh";

        private readonly WorldModel _worldModel;
        private readonly NavigationSystem _navigationSystem;
        private readonly ServerSystemCollection _serverSystems;

        private DtNavMesh _navMesh;
        private DtNavMeshQuery _query;

        public NavigationPresenter(WorldModel worldModel)
        {
            _worldModel = worldModel;
            _navigationSystem = new NavigationSystem(worldModel);
            _serverSystems = worldModel.ServerSystems;
        }

        public void Enable()
        {
            LoadNavMesh();

            _worldModel.Sessions.OnAdded += HandleSessionAdd;

            foreach (var session in _worldModel.Sessions.Models.Values)
            {
                HandleSessionAdd(session);
            }

            _serverSystems.Add(_navigationSystem);
        }

        public void Disable()
        {
            _worldModel.Sessions.OnAdded -= HandleSessionAdd;

            _serverSystems.Remove(_navigationSystem);
        }

        private void LoadNavMesh()
        {
            using var stream = new FileStream(_filename, FileMode.Open);
            using var br = new BinaryReader(stream);

            var reader = new DtMeshSetReader();
            _navMesh = reader.Read(br, 6);
            _query = new DtNavMeshQuery(_navMesh);
        }

        private void HandleSessionAdd(SessionModel session)
        {
            session.NavMesh.NavMesh = _navMesh;
            session.NavMesh.Query = _query;
        }
    }
}
