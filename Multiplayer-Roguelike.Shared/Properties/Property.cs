using System;
using System.Collections.Generic;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Properties
{
    public class Property<T> : ISharedData
    {
        public event Action OnChange;

        public string Id { get; }

        public bool IsDirty { get; private set; }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    IsDirty = true;
                    OnChange?.Invoke();
                }
            }
        }

        private T _value;

        public Property(string id, T value)
        {
            Id = id;
            _value = value;
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out T value);
            Value = value;
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
