using Runtime.ECS.Components.Battle.Weapon;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Core;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI
{
    public class UIDrawSwitchWeapon : BaseSystem
    {
        private QueryBuffer<SwitchWeaponEventComponent, WeaponSlotsComponent, LocalControllableTag> _buffer = new();
        private const string _selectedClass = "weapon-panel-selected";

        private readonly VisualElement _meleeWeapon;
        private readonly VisualElement _rangeWeapon;

        public UIDrawSwitchWeapon(UIHudView hudView)
        {
            _meleeWeapon = hudView.HudRoot.Q<VisualElement>("melee-weapon-panel");
            _rangeWeapon = hudView.HudRoot.Q<VisualElement>("range-weapon-panel");
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var current = _buffer.Components1[i];
                var slots = _buffer.Components2[i];

                if (slots.SlotEntityIds.Length < 2)
                {
                    continue;
                }

                var isMelee = current.TargetSlot == 0;

                SetSelected(_meleeWeapon, isMelee);
                SetSelected(_rangeWeapon, !isMelee);
            }
        }

        private void SetSelected(VisualElement element, bool selected)
        {
            if (selected)
            {
                element.AddToClassList(_selectedClass);
            }
            else
            {
                element.RemoveFromClassList(_selectedClass);
            }
        }
    }
}
