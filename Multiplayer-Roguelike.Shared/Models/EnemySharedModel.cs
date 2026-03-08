using Shared.Models.Common;
using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models
{
    public class EnemySharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 0f);
        public readonly Property<Vector3> Position = new Property<Vector3>("last_position", new Vector3(0f, 0f, 0f));

        public EnemySharedModel(string id) : base(id)
        {
            Children.Add(Health.Id, Health);
            Children.Add(Position.Id, Position);
        }

        public static EnemySharedModel Create(string id)
        {
            return new EnemySharedModel(id);
        }
    }
}
