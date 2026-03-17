using System;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchNetworkSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<CharacterNetworkSyncComponent, NetworkControllableTag, CurrentWeaponComponent, WeaponSlotsComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var networkSyncComponent = _buffer.Components1[i];
            var currentWeaponComponent = _buffer.Components3[i];
            var weaponSlots = _buffer.Components4[i];

            var targetSlot = networkSyncComponent.CharacterSharedModel.EquippedWeaponSlotId.Value;

            var currentWeaponSlot = Array.IndexOf(weaponSlots.SlotEntityIds, currentWeaponComponent.WeaponEntityId);
            var hasDifferent = targetSlot != currentWeaponSlot;

            if (hasDifferent)
            {
                ComponentManager.AddComponent(entityId, new SwitchWeaponEventComponent(targetSlot));
            }
        }
    }
}
