using System.IO;
using Backend.Enemies;
using Backend.Session;
using DotRecast.Detour;
using DotRecast.Detour.Io;

namespace Backend.Navigation
{
    public class NavigationPresenter : IPresenter
    {
        private const string _filename = "Resources/base.navmesh";

        private readonly WorldModel _worldModel;
        private readonly NavigationSystem _navigationSystem;
        private readonly EnemyPositionSyncSystem _enemyPositionSyncSystem;

        private DtNavMesh _navMesh;

        public NavigationPresenter(WorldModel worldModel)
        {
            _worldModel = worldModel;
            _navigationSystem = new NavigationSystem("navigation-system");
            _enemyPositionSyncSystem = new EnemyPositionSyncSystem("enemy-position-sync-system");
        }

        public void Enable()
        {
            LoadNavMesh();

            _worldModel.Sessions.OnAdded += HandleSessionAdded;
            _worldModel.Sessions.OnRemoved += HandleSessionRemoved;

            foreach (var session in _worldModel.Sessions.Models.Values)
            {
                HandleSessionAdded(session);
            }

            _worldModel.ServerSystems.Add(_navigationSystem);
            _worldModel.ServerSystems.Add(_enemyPositionSyncSystem);
        }

        public void Disable()
        {
            _worldModel.Sessions.OnAdded -= HandleSessionAdded;
            _worldModel.Sessions.OnRemoved -= HandleSessionRemoved;
            _worldModel.ServerSystems.Remove(_enemyPositionSyncSystem);
            _worldModel.ServerSystems.Remove(_navigationSystem);
        }

        private void LoadNavMesh()
        {
            using var stream = new FileStream(_filename, FileMode.Open);
            using var br = new BinaryReader(stream);

            var reader = new DtMeshSetReader();
            _navMesh = reader.Read(br, 6);
        }

        private void HandleSessionAdded(GameSessionModel gameSession)
        {
            gameSession.SetupNavigation(_navMesh);
            _navigationSystem.Register(gameSession);
            _enemyPositionSyncSystem.Register(gameSession);
        }

        private void HandleSessionRemoved(GameSessionModel gameSession)
        {
            _enemyPositionSyncSystem.Unregister(gameSession);
            _navigationSystem.Unregister(gameSession);
            gameSession.Navigation = null;
        }
    }
}
