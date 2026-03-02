using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Models
{
    public sealed class SharedModelCollection<T> : ISharedData where T : ISharedData
    {
        public string Id { get; }

        public bool IsDirty =>
            _cleared ||
            _added.Count > 0 ||
            _removed.Count > 0 ||
            _models.Values.Any(model => model.IsDirty);


        private readonly Dictionary<string, T> _models = new Dictionary<string, T>();
        public IEnumerable<T> Models => _models.Values;

        private readonly List<T> _added = new List<T>();
        private readonly List<T> _removed = new List<T>();
        private readonly Func<string, T> _factory;

        private bool _cleared;

        public SharedModelCollection(string id, Func<string, T> factory)
        {
            _factory = factory;
            Id = id;
        }

        public void Add(T model)
        {
            if (_models.TryAdd(model.Id, model))
            {
                _added.Add(model);
            }
        }

        public void Remove(T model)
        {
            if (_models.Remove(model.Id))
            {
                if (!_added.Remove(model))
                {
                    _removed.Add(model);
                }
            }
        }

        public bool TryGet(string id, out T model)
        {
            return _models.TryGetValue(id, out model);
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out bool cleared);
            if (cleared)
            {
                _models.Clear();
            }

            protocol.Get(out int addedCount);
            for (var i = 0; i < addedCount; i++)
            {
                protocol.Get(out string id);
                var model = _factory.Invoke(id);
                model.Read(protocol);
                _models[id] = model;
            }

            protocol.Get(out int removedCount);
            for (var i = 0; i < removedCount; i++)
            {
                protocol.Get(out string id);
                _models.Remove(id);
            }

            protocol.Get(out int updatedCount);
            for (var i = 0; i < updatedCount; i++)
            {
                protocol.Get(out string id);
                _models[id].Read(protocol);
            }
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);

            protocol.Add(_cleared);

            protocol.Add(_added.Count);
            foreach (var model in _added)
            {
                model.Write(protocol);
            }

            protocol.Add(_removed.Count);
            foreach (var model in _removed)
            {
                protocol.Add(model.Id);
            }

            var updated = _models.Values.Where(model => model.IsDirty && !_added.Contains(model)).ToArray();
            protocol.Add(updated.Length);
            foreach (var model in updated)
            {
                if (!_added.Contains(model) && !_removed.Contains(model))
                {
                    model.Write(protocol);
                }
            }
        }

        public void Clear()
        {
            _models.Clear();
            _added.Clear();
            _removed.Clear();
            _cleared = true;
        }

        public void ClearDirty()
        {
            _added.Clear();
            _removed.Clear();
            _cleared = false;
            foreach (var model in _models.Values)
            {
                model.ClearDirty();
            }
        }
    }
}
