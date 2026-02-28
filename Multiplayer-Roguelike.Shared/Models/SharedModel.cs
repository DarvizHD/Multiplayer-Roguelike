using System.Collections.Generic;
using System.Linq;
using Shared.Properties;
using Shared.Protocol;

namespace Shared.Models
{
    public abstract class SharedModel
    {
        public string Id { get; }

        public Dictionary<string, IProperty> Properties { get; } = new Dictionary<string, IProperty>();
        public Dictionary<string, SharedModel> Models { get; } = new Dictionary<string, SharedModel>();

        public bool IsDirty => Properties.Values.Any(p => p.IsDirty) || Models.Values.Any(m => m.IsDirty);

        protected SharedModel(string id)
        {
            Id = id;
        }

        public void GetChanges(out Dictionary<string, object> changes)
        {
            changes = new Dictionary<string, object>();
            foreach (var property in Properties.Values.Where(p => p.IsDirty))
            {
                changes.Add(property.Id, property);
            }

            foreach (var model in Models.Values.Where(m => m.IsDirty))
            {
                changes.Add(model.Id, model);
                model.GetChanges(out var innerChanges);
                foreach (var change in innerChanges)
                {
                    changes.Add(change.Key, change.Value);
                }
            }
        }

        public void Read(NetworkProtocol protocol)
        {
            protocol.Get(out int propertyLenght);
            for (var i = 0; i < propertyLenght; i++)
            {
                protocol.Get(out string propertyId);
                Properties[propertyId].Read(protocol);
            }

            protocol.Get(out int modelsLenght);
            for (var i = 0; i < modelsLenght; i++)
            {
                protocol.Get(out string modelId);
                Models[modelId].Read(protocol);
            }
        }

        public void Write(NetworkProtocol protocol)
        {
            protocol.Add(Id);

            var changedProperties = Properties.Values.Where(p => p.IsDirty).ToList();
            protocol.Add(changedProperties.Count());
            foreach (var property in changedProperties)
            {
                property.Write(protocol);
                property.UnsetDirty();
            }

            var changedModels = Models.Values.Where(m => m.IsDirty).ToList();
            protocol.Add(changedModels.Count());
            foreach (var model in changedModels)
            {
                model.Write(protocol);
            }
        }
    }
}
