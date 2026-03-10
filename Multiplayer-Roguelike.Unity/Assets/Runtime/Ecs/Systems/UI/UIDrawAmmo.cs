using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawAmmo : BaseSystem
    {
        private QueryBuffer<WeaponSlotsComponent, LocalControllableTag> _buffer = new();
        private readonly Label _currentAmmo;
        private readonly Label _maxAmmo;

        public UIDrawAmmo(UIHudView hudView)
        {
            var rangeWeapon = hudView.HudRoot.Q<VisualElement>("range-weapon-panel");
            _currentAmmo = rangeWeapon.Q<Label>("current-value");
            _maxAmmo = rangeWeapon.Q<Label>("max-value");
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var slots = _buffer.Components1[i];

                RangedWeaponComponent ranged = null;
                AmmoComponent ammo = null;

                foreach (var slotEntityId in slots.SlotEntityIds)
                {
                    if (!ComponentManager.TryGetComponent<RangedWeaponComponent>(slotEntityId, out var rangedWeaponComponent))
                    {
                        continue;
                    }

                    if (!ComponentManager.TryGetComponent<AmmoComponent>(slotEntityId, out var ammoComponent))
                    {
                        continue;
                    }

                    ranged = rangedWeaponComponent;
                    ammo = ammoComponent;
                    break;
                }

                if (ranged == null || ammo == null)
                {
                    _currentAmmo.text = string.Empty;
                    continue;
                }

                _currentAmmo.text = ranged.IsReloading ? "..." : ammo.Current.ToString();
                _maxAmmo.text = ammo.Reserve.ToString();
            }
        }
    }
}
