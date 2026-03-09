using Runtime.ECS.Components;
using Runtime.Tools;

namespace Runtime.ECS.Systems
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
