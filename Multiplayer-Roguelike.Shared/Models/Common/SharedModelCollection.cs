using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Models.Common
{
    public abstract class SharedModelCollection<T> : ISharedData where T : ISharedData
    {
        public event Action<T> Added;
        public event Action<string> Removed;

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

        private bool _cleared;

        public SharedModelCollection(string id)
        {
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
            protocol.Get(out string _); // читает Id
            ReadData(protocol);
        }

        public void ReadData(NetworkProtocol protocol)
        {
            System.Console.WriteLine($"[SharedModelCollection.ReadData] Collection '{Id}': starting read at position {protocol.Stream.Position}");

            protocol.Get(out bool cleared);
            System.Console.WriteLine($"[SharedModelCollection.ReadData] Cleared: {cleared}");
            if (cleared) _models.Clear();

            protocol.Get(out int addedCount);
            System.Console.WriteLine($"[SharedModelCollection.ReadData] Added count: {addedCount}");

            for (var i = 0; i < addedCount; i++)
            {
                var posBeforeId = protocol.Stream.Position;
                protocol.Get(out string id);
                System.Console.WriteLine($"[SharedModelCollection.ReadData] Reading model {i+1}/{addedCount}: ID='{id}' at position {posBeforeId}");

                var model = CreateInstance(id);
                model.ReadData(protocol); // читаем только данные, ID уже прочитан
                System.Console.WriteLine($"[SharedModelCollection.ReadData] After model '{id}' ReadData, pos: {protocol.Stream.Position}");
                _models[id] = model;
                Added?.Invoke(model);
            }

            protocol.Get(out int removedCount);
            System.Console.WriteLine($"[SharedModelCollection.ReadData] Removed count: {removedCount}");
            for (var i = 0; i < removedCount; i++)
            {
                protocol.Get(out string id);
                _models.Remove(id);
                Removed?.Invoke(id);
            }

            protocol.Get(out int updatedCount);
            System.Console.WriteLine($"[SharedModelCollection.ReadData] Updated count: {updatedCount}");
            for (var i = 0; i < updatedCount; i++)
            {
                protocol.Get(out string id);
                if (!_models.TryGetValue(id, out var model))
                {
                    model = CreateInstance(id);
                    _models[id] = model;
                    Added?.Invoke(model);
                }
                model.ReadData(protocol); // уже без Id
            }

            System.Console.WriteLine($"[SharedModelCollection.ReadData] Collection '{Id}': finished read at position {protocol.Stream.Position}");
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

        public void WriteAll(NetworkProtocol protocol)
        {
            protocol.Add(Id);
            protocol.Add(false);

            protocol.Add(_models.Values.Count);
            foreach (var model in _models.Values)
            {
                model.WriteAll(protocol);
            }

            protocol.Add(0);
            protocol.Add(0);
        }

        protected abstract T CreateInstance(string id);

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
