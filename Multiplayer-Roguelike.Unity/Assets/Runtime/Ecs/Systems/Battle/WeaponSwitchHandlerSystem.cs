using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchHandlerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<SwitchWeaponEventComponent, WeaponSlotsComponent, CurrentWeaponComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
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
