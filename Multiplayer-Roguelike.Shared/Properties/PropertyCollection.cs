using System;
using System.Collections.Generic;
using System.Text;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Properties
{
    public class PropertyCollection<T> : ISharedData
    {
        public string Id { get; }
        public List<T> Values { get; } = new List<T>();

        public event Action<T> OnAdded;
        public event Action<T> OnRemoved;

        public bool IsDirty =>
            _cleared ||
            _added.Count > 0 ||
            _removed.Count > 0;

        private readonly List<T> _added = new List<T>();
        private readonly List<T> _removed = new List<T>();

        private bool _cleared;

        public PropertyCollection(string id)
        {
            Id = id;
        }

        public void Add(T value)
        {
            Values.Add(value);
            _added.Add(value);
        }

        public void Remove(T value)
        {
            if (Values.Remove(value))
            {
                if (_added.Remove(value))
                {
                    return;
                }

                _removed.Add(value);
            }
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out bool cleared);
            if (cleared)
            {
                Values.Clear();
            }

            protocol.Get(out int addCount);
            for (var i = 0; i < addCount; i++)
            {
                protocol.Get(out T value);
                Values.Add(value);
                OnAdded?.Invoke(value);
            }

            protocol.Get(out int removeCount);
            for (var i = 0; i < removeCount; i++)
            {
                protocol.Get(out T value);
                Values.Remove(value);
                OnRemoved?.Invoke(value);
            }
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);

            protocol.Add(_cleared);

            protocol.Add(_added.Count);
            foreach (var value in _added)
            {
                protocol.Add(value);
            }

            protocol.Add(_removed.Count);
            foreach (var value in _removed)
            {
                protocol.Add(value);
            }
        }

        public void Clear()
        {
            Values.Clear();
            _added.Clear();
            _removed.Clear();
            _cleared = true;
        }

        public void ClearDirty()
        {
            _added.Clear();
            _removed.Clear();
            _cleared = false;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            foreach (var value in Values)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(',');
            }

            stringBuilder[^1] = ']';
            return stringBuilder.ToString();
        }
    }
}
