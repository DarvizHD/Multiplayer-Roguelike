using Runtime.Ecs.Components;
using Runtime.Tools;

namespace Runtime.Ecs.Systems.Weapons
{
    public class WeaponProviderComponent : IComponent
    {
        public WeaponProvider WeaponProvider;

        public WeaponProviderComponent(WeaponProvider weaponProvider)
        {
            WeaponProvider = weaponProvider;
        }
    }
}
