using System;
using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawCrosshairSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<CursorWorldPositionComponent, WeaponSlotsComponent, CurrentWeaponComponent, LocalControllableTag> _buffer = new();

        private readonly UIHudView _hudView;
        private VisualElement _crosshair;
        private readonly Camera _camera;

        public UIDrawCrosshairSystem(UIHudView hudView)
        {
            _hudView = hudView;
            _camera = Camera.main;
        }

        protected override void Update(int i, float deltaTime)
        {
            var cursorWorldPositionComponent = _buffer.Components1[i];
            var weaponSlotsComponent = _buffer.Components2[i];
            var currentWeaponComponent = _buffer.Components3[i];

            var slot = Array.IndexOf(weaponSlotsComponent.SlotEntityIds, currentWeaponComponent.WeaponEntityId);

            if (_crosshair == null)
            {
                _crosshair = _hudView.CrosshairAsset.CloneTree().Q<VisualElement>("crosshair");
                _hudView.HudRoot.Add(_crosshair);
            }

            var visible = slot == 1;

            Cursor.visible = !visible;
            _crosshair.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;


            var mousePosition = _camera.WorldToScreenPoint(cursorWorldPositionComponent.Position);

            var x =  mousePosition.x;
            var y = Screen.height - mousePosition.y;

            _crosshair.style.left = x - _crosshair.resolvedStyle.width / 2;
            _crosshair.style.top = y - _crosshair.resolvedStyle.height / 2;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        public void Destroy()
        {
            _hudView.HudRoot.Remove(_crosshair);
        }
    }
}
