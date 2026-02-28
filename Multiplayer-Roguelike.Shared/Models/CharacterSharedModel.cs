using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models
{
    public class CharacterSharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 0f);
        public readonly Property<Vector3> Direction = new Property<Vector3>("direction", new Vector3(0f, 0f, 0f));
        public readonly Property<Vector3> Rotation = new Property<Vector3>("rotation", new Vector3(0f, 0f, 0f));

        public CharacterSharedModel(string id) : base(id)
        {
            Properties.Add(Health.Id, Health);
            Properties.Add(Direction.Id, Direction);
            Properties.Add(Rotation.Id, Rotation);
        }
    }
}
