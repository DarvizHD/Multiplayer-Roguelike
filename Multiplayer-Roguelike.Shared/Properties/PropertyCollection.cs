using System.Collections.Generic;
using System.Text;
using Shared.Protocol;

namespace Shared.Properties
{
    public class PropertyCollection<T> : IProperty
    {
        public string Id { get; }

        private readonly List<T> _values = new List<T>();

        public bool IsDirty { get; private set; }

        public PropertyCollection(string id)
        {
            Id = id;
        }

        public void Add(T value)
        {
            _values.Add(value);
            IsDirty = true;
        }

        public void Remove(T value)
        {
            _values.Remove(value);
            IsDirty = true;
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
            foreach (var value in _values)
            {
                protocol.Add(value);
            }
        }

        public void Clear()
        {
            _values.Clear();
            IsDirty = true;
        }

        public void UnsetDirty()
        {
            IsDirty = false;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            foreach (var value in _values)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(',');
            }

            stringBuilder[^1] = ']';
            return stringBuilder.ToString();
        }
    }
}
