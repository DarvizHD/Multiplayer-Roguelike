using System.Collections.Generic;
using System.Text;
using Shared.Protocol;

namespace Shared.Properties
{
    public class PropertyCollection<T> : IProperty
    {
        public string Id { get; }

        private readonly List<T> _values = new List<T>();

        private bool _isDirty;
        public bool IsDirty => _isDirty;

        public PropertyCollection(string id)
        {
            Id = id;
        }

        public void Add(T value)
        {
            _values.Add(value);
            _isDirty = true;
        }

        public void Remove(T value)
        {
            _values.Remove(value);
            _isDirty = true;
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out int length);
            for (var i = 0; i < length; i++)
            {
                protocol.Get(out T value);
                _values.Add(value);
            }
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);
            protocol.Add(_values.Count);
            foreach (T value in _values)
            {
                protocol.Add(value);
            }
        }

        public void Clear()
        {
            _values.Clear();
            _isDirty = true;
        }

        public void UnsetDirty()
        {
            _isDirty = false;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            foreach (T value in _values)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(',');
            }

            stringBuilder[^1] = ']';
            return stringBuilder.ToString();
        }
    }
}
