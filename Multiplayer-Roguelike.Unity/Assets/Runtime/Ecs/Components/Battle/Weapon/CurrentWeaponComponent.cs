namespace Runtime.Ecs.Components.Battle.Weapon
{
    public class CurrentWeaponComponent : IComponent
    {
        public ushort WeaponEntityId { get; set; }

        public CurrentWeaponComponent(ushort weaponEntityId)
        {
            WeaponEntityId = weaponEntityId;
        }
    }
}
