namespace Runtime.Ecs.Components.Battle.Weapon
{
    public class RangedWeaponComponent : IComponent
    {
        public float Damage { get; }
        public float AimRadius { get; }
        public bool IsReloading { get; set; }
        public float ReloadTime { get; }
        public float ReloadTimer { get; set; }

        public RangedWeaponComponent(float damage, float aimRadius, float reloadTime)
        {
            Damage = damage;
            AimRadius = aimRadius;
            ReloadTime = reloadTime;
        }
    }
}
