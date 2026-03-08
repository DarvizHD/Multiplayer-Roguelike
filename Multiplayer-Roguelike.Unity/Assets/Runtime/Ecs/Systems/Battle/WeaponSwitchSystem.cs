using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchSystem : BaseSystem
    {
        private QueryBuffer<SwitchWeaponEventComponent, WeaponSlotsComponent, CurrentWeaponComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var switchEvent = _buffer.Components1[i];
                var slots = _buffer.Components2[i];
                var current = _buffer.Components3[i];

                var targetSlot = switchEvent.TargetSlot;

                if (targetSlot >= 0 && targetSlot < slots.SlotEntityIds.Length)
                {
                    current.WeaponEntityId = slots.SlotEntityIds[targetSlot];
                }

                ComponentManager.RemoveComponent<SwitchWeaponEventComponent>(entityId);
            }
        }
    }
}
