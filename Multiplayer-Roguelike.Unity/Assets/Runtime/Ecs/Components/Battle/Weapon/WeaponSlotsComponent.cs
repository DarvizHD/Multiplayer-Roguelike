namespace Runtime.ECS.Components.Battle.Weapon
{
    public class WeaponSlotsComponent : IComponent
    {
        public ushort[] SlotEntityIds { get; }



        public WeaponSlotsComponent(ushort[] slotEntityIds)
        {
            SlotEntityIds = slotEntityIds;
        }
    }
}
