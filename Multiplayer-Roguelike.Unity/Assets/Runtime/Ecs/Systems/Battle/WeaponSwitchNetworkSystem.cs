using System;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using UnityEngine;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchNetworkSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, NetworkControllableTag, CurrentWeaponComponent, WeaponSlotsComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var networkSyncComponent = _buffer.Components1[i];
                var currentWeaponComponent = _buffer.Components3[i];
                var weaponSlots = _buffer.Components4[i];

                var targetSlot = networkSyncComponent.CharacterSharedModel.EquippedWeaponSlotId.Value;

                var currentWeaponSlot = Array.IndexOf(weaponSlots.SlotEntityIds, currentWeaponComponent.WeaponEntityId);
                var hasDifferent = targetSlot != currentWeaponSlot;

                Debug.Log($"Network player: {networkSyncComponent.CharacterSharedModel.Id}Current slot: {currentWeaponSlot} | Target slot: {targetSlot}");

                if (hasDifferent)
                {
                    Debug.Log($"NETWORK SWITCH {entityId}: network: {targetSlot} current: {currentWeaponSlot}");
                    ComponentManager.AddComponent(entityId, new SwitchWeaponEventComponent(targetSlot));
                }
            }
        }
    }
}
