using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Weapons
{
    public class DrawWeaponSwitcherSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<SwitchWeaponEventComponent, WeaponProviderComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var switchEventComponent = _buffer.Components1[i];
            var weaponProviderComponent = _buffer.Components2[i];

            weaponProviderComponent.WeaponProvider.Current = switchEventComponent.TargetSlot;
        }
    }
}
