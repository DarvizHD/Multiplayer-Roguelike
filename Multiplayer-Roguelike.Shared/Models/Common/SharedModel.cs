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
            protocol.Get(out string _);
            ReadData(protocol);
        }

        public void ReadData(NetworkProtocol protocol)
        {
            protocol.Get(out int count);
            System.Console.WriteLine($"[SharedModel.ReadData] Model '{Id}': reading {count} properties. Stream position: {protocol.Stream.Position}");

            for (var i = 0; i < count; i++)
            {
                var positionBefore = protocol.Stream.Position;
                protocol.Get(out string propertyId);

                if (Children.TryGetValue(propertyId, out var child))
                {
                    child.ReadData(protocol);
                }
                else
                {
                    var availableProps = string.Join(", ", Children.Keys);
                }
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

        public void WriteAll(NetworkProtocol protocol)
        {
            protocol.Add(Id);

            protocol.Add(Children.Count);
            foreach (var child in Children.Values)
            {
                child.WriteAll(protocol);
                child.ClearDirty();
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
