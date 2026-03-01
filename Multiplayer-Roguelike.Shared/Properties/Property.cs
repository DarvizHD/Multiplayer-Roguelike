using System.Collections.Generic;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Properties
{
    public class Property<T> : ISharedData
    {
        public string Id { get; }

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    IsDirty = true;
                }
            }
        }

        public bool IsDirty { get; private set; }

        public Property(string id, T value)
        {
            Id = id;
            _value = value;
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out _value);
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);
            protocol.Add(_value);
        }

        public void ClearDirty()
        {
            IsDirty = false;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
