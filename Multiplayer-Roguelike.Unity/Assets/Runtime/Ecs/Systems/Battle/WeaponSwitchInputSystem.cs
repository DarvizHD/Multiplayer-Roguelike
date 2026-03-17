using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchInputSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PlayerInputComponent, WeaponSlotsComponent,
            CurrentWeaponComponent, LocalControllableTag> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var input = _buffer.Components1[i];
            var weaponSlots = _buffer.Components2[i];
            var currentWeaponComponent = _buffer.Components3[i];

            var targetSlot = -1;

            if (input.PlayerControls.Gameplay.Interact.IsPressed())
            {
                ComponentManager.AddComponent(entityId, new ReloadEventComponent());
            }

            if (input.PlayerControls.Gameplay.Previous.IsPressed())
            {
                targetSlot = 0;
            }

            if (input.PlayerControls.Gameplay.Next.IsPressed())
            {
                targetSlot = 1;
            }

            if (targetSlot == -1)
            {
                return;
            }

            if (weaponSlots.SlotEntityIds[targetSlot] == currentWeaponComponent.WeaponEntityId)
            {
                return;
            }

            ComponentManager.AddComponent(entityId, new SwitchWeaponEventComponent(targetSlot));
        }
    }
}
