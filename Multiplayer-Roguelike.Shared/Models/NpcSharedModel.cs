using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models
{
    public class NpcSharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 0f);
        public readonly Property<Vector3> LastPosition = new Property<Vector3>("last_position", new Vector3(0f, 0f, 0f));

        public NpcSharedModel(string id) : base(id)
        {
            Children.Add(Health.Id, Health);
            Children.Add(LastPosition.Id, LastPosition);
        }

        public static NpcSharedModel Create(string id)
        {
            return new NpcSharedModel(id);
        }
    }
}
