using System.Collections.Generic;
using System.Linq;
using Shared.Common;
using Shared.Protocol;

namespace Shared.Models.Common
{
    public abstract class SharedModel : ISharedData
    {
        public string Id { get; }

        public Dictionary<string, ISharedData> Children { get; } = new Dictionary<string, ISharedData>();

        public bool IsDirty => Children.Values.Any(p => p.IsDirty);

        protected SharedModel(string id)
        {
            Id = id;
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out int propertyLenght);
            for (var i = 0; i < propertyLenght; i++)
            {
                protocol.Get(out string propertyId);
                Children[propertyId].Read(protocol);
            }
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);

            var changedProperties = Children.Values.Where(p => p.IsDirty).ToList();
            protocol.Add(changedProperties.Count);
            foreach (var property in changedProperties)
            {
                property.Write(protocol);
                property.ClearDirty();
            }
        }

        public void ClearDirty()
        {
            foreach (var child in Children.Values)
            {
                child.ClearDirty();
            }
        }
    }
}
