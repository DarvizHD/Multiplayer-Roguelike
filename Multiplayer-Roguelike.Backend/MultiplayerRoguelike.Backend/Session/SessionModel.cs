using Backend.Enemies;
using Backend.Navigation;
using Backend.Player.Collection;
using Backend.Session.SpawnDirector;
using DotRecast.Detour;
using Shared.Models.GameSession;

namespace Backend.Session
{
    public class SessionModel
    {
        public string Id { get; }

        public PlayerModelCollection Players { get; } = new();

        public GameSessionSharedModel GameSessionSharedModel { get; }

        public EnemyModelCollection Enemies { get; } = new();

        public NavigationModel Navigation { get; set; }

        public SpawnDirectorModel SpawnDirector { get; set; }
        public GameSessionWaveModel GameSessionWaveModel { get; set; }

        public SessionModel(string id)
        {
            Id = id;
            GameSessionSharedModel = new GameSessionSharedModel(id);
            SpawnDirector = new SpawnDirectorModel(new SpawnDirectorConfig());
            GameSessionWaveModel = new GameSessionWaveModel();
        }

        public void SetupNavigation(DtNavMesh navMesh)
        {
            Navigation = new NavigationModel(navMesh);
            Navigation.SetupObstacleAvoidance();
        }
    }
}
