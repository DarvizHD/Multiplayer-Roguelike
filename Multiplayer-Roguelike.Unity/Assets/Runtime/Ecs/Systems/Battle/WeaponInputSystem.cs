using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Player;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponInputSystem : BaseSystem
    {
        private QueryBuffer<PlayerInputComponent, WeaponSlotsComponent, CurrentWeaponComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var input = _buffer.Components1[i];

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

                ComponentManager.AddComponent(entityId, new SwitchWeaponEventComponent(targetSlot));
            }
        }
    }
}
