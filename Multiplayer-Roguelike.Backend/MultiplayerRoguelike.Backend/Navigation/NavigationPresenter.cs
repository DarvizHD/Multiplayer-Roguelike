using Backend.ServerSystems;

namespace Backend.Navigation
{
    public class NavigationPresenter : IPresenter
    {
        private readonly NavigationSystem _navigationSystem;
        private readonly ServerSystemCollection _serverSystems;

        public NavigationPresenter(WorldModel worldModel)
        {
            _navigationSystem = new NavigationSystem(worldModel);
            _serverSystems = worldModel.ServerSystems;
        }

        public void Enable()
        {
            _serverSystems.Add(_navigationSystem);
        }

        public void Disable()
        {
            _serverSystems.Remove(_navigationSystem);
        }
    }
}
