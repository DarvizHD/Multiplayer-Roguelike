using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.Battle
{
    public class WeaponSwitchInputSystem : BaseSystem
    {
        private QueryBuffer<PlayerInputComponent, WeaponSlotsComponent,
            CurrentWeaponComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
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
                    continue;
                }

                if (weaponSlots.SlotEntityIds[targetSlot] == currentWeaponComponent.WeaponEntityId)
                {
                    continue;
                }

                ComponentManager.AddComponent(entityId, new SwitchWeaponEventComponent(targetSlot));
            }
        }
    }
}
