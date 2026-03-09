using Runtime.ECS.Components;
using Runtime.Tools;

namespace Runtime.Ecs.Systems
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
