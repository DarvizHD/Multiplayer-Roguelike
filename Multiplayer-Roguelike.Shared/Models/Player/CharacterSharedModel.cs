using Shared.Models.Common;
using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models.Player
{
    public class CharacterSharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 100f);
        public readonly Property<Vector3> Position = new Property<Vector3>("position", new Vector3(0f, 0f, 0f));
        public readonly Property<Vector3> Direction = new Property<Vector3>("direction", new Vector3(0f, 0f, 0f));
        public readonly Property<float> Rotation = new Property<float>("rotation", 0f);
        public readonly Property<ushort> EquippedWeaponSlotId = new Property<ushort>("equipped_weapon_slot", 0);
        public readonly Property<string> EventId = new Property<string>("event_id", string.Empty);

        public CharacterSharedModel(string id) : base(id)
        {
            Children.Add(Health.Id, Health);
            Children.Add(Position.Id, Position);
            Children.Add(Direction.Id, Direction);
            Children.Add(Rotation.Id, Rotation);
            Children.Add(EquippedWeaponSlotId.Id, EquippedWeaponSlotId);
            Children.Add(EventId.Id, EventId);
        }
    }
}
