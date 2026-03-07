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

        public NavigationPresenter(WorldModel worldModel)
        {
            _worldModel = worldModel;
            _navigationSystem = new NavigationSystem(worldModel);
            _serverSystems = worldModel.ServerSystems;
        }

        public void Enable()
        {
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

        private void HandleSessionAdd(SessionModel session)
        {
            using var stream = new FileStream(_filename, FileMode.Open);
            using var br = new BinaryReader(stream);

            var reader = new DtMeshSetReader();
            var mesh = reader.Read(br, 6);

            session.NavMesh.NavMesh = mesh;
            session.NavMesh.Query = new DtNavMeshQuery(mesh);
        }
    }
}
