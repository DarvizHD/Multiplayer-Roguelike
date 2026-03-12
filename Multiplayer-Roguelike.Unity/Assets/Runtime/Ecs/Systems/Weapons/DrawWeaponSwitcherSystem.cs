using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Weapons
{
    public class DrawWeaponSwitcherSystem : BaseSystem
    {
        private QueryBuffer<SwitchWeaponEventComponent, WeaponProviderComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var switchEventComponent = _buffer.Components1[i];
                var weaponProviderComponent = _buffer.Components2[i];

                weaponProviderComponent.WeaponProvider.Current = switchEventComponent.TargetSlot;
            }
        }
    }
}
