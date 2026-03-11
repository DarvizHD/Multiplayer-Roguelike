using Runtime.ECS.Components;
using UnityEngine;

namespace Runtime.Ecs.Components.Battle.Weapon
{
    public class RangedWeaponComponent : IComponent
    {
        public float Damage { get; }
        public float AimRadius { get; }
        public bool IsReloading { get; set; }
        public float ReloadTime { get; }
        public float ReloadTimer { get; set; }
        public AudioClip ShootClip { get; }
        public AudioClip ReloadClip { get; }

        public RangedWeaponComponent(float damage, float aimRadius, float reloadTime, AudioClip shootClip, AudioClip reloadClip)
        {
            Damage = damage;
            AimRadius = aimRadius;
            ReloadTime = reloadTime;
            ShootClip = shootClip;
            ReloadClip = reloadClip;
        }
    }
}
