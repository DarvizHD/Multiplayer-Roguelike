using Shared.Models.Common;
using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models
{
    public class CharacterSharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 0f);
        public readonly Property<Vector3> Position = new Property<Vector3>("position", new Vector3(0f, 0f, 0f));
        public readonly Property<Vector3> Direction = new Property<Vector3>("direction", new Vector3(0f, 0f, 0f));
        public readonly Property<float> Rotation = new Property<float>("rotation", 0f);

        public CharacterSharedModel(string id) : base(id)
        {
            Children.Add(Health.Id, Health);
            Children.Add(Position.Id, Position);
            Children.Add(Direction.Id, Direction);
            Children.Add(Rotation.Id, Rotation);
        }

        public static CharacterSharedModel Create(string id)
        {
            return new CharacterSharedModel(id);
        }
    }
}
