using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawSwitchWeaponSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<SwitchWeaponEventComponent, WeaponSlotsComponent, LocalControllableTag> _buffer = new();
        private const string _selectedClass = "weapon-panel-selected";

        private readonly VisualElement _meleeWeapon;
        private readonly VisualElement _rangeWeapon;

        public UIDrawSwitchWeaponSystem(UIHudView hudView)
        {
            _meleeWeapon = hudView.HudRoot.Q<VisualElement>("melee-weapon-panel");
            _rangeWeapon = hudView.HudRoot.Q<VisualElement>("range-weapon-panel");
        }


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var current = _buffer.Components1[i];
            var slots = _buffer.Components2[i];

            if (slots.SlotEntityIds.Length < 2)
            {
                return;
            }

            var isMelee = current.TargetSlot == 0;

            SetSelected(_meleeWeapon, isMelee);
            SetSelected(_rangeWeapon, !isMelee);
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
