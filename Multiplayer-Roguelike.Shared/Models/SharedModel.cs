using System.Collections.Generic;
using System.Linq;
using Shared.Properties;

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
                property.UnsetDirty();
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
    }
}
