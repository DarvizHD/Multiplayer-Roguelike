using System.Collections.Generic;

namespace Backend.ServerSystems
{
    public class ServerSystemCollection
    {
        private readonly Dictionary<string, IServerSystem> _systems = new();
        private readonly List<IServerSystem> _pendingAdd = new();
        private readonly List<IServerSystem> _pendingRemove = new();

        private bool _isUpdating;

        public IServerSystem Get(string id)
        {
            return _systems[id];
        }

        public void Add(IServerSystem system)
        {
            if (_isUpdating)
            {
                _pendingAdd.Add(system);
                return;
            }

            _systems.Add(system.Id, system);
        }

        public void Remove(IServerSystem system)
        {
            if (_isUpdating)
            {
                _pendingRemove.Add(system);
                return;
            }

            _systems.Remove(system.Id);
        }

        public void Update(float deltaTime)
        {
            _isUpdating = true;

            foreach (var system in _systems.Values)
            {
                system.Update(deltaTime);
            }

            _isUpdating = false;

            if (_pendingRemove.Count > 0)
            {
                foreach (var system in _pendingRemove)
                {
                    _systems.Remove(system.Id);
                }

                _pendingRemove.Clear();
            }

            if (_pendingAdd.Count > 0)
            {
                foreach (var system in _pendingAdd)
                {
                    _systems.Add(system.Id, system);
                }

                _pendingAdd.Clear();
            }
        }

        public void Clear()
        {
            _systems.Clear();
            _pendingAdd.Clear();
            _pendingRemove.Clear();
        }
    }
}
