using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Names
{
    public class UINameDrawSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<UIComponent, NameComponent, PositionComponent, AliveTagComponent> _buffer = new();
        private readonly Camera _camera = Camera.main;
        private readonly UIHudView _hudView;

        public UINameDrawSystem(UIHudView hudView)
        {
            _hudView = hudView;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var uiComponent = _buffer.Components1[i];
            var positionComponent = _buffer.Components3[i];

            if (!uiComponent.Elements.Contains(UIConstants.Nickname))
            {
                return;
            }

            var id = $"{UIConstants.Nickname}_{entityId}";
            var label = _hudView.WorldHudRoot.Q<Label>(id);

            if (label == null)
            {
                return;
            }

            var panel = _hudView.WorldHudRoot.panel;
            if (panel == null)
            {
                return;
            }

            var screenPos = _camera.WorldToScreenPoint(positionComponent.Position);

            var outScreen = screenPos.z <= 0 ||
                            screenPos.x < 0 || screenPos.x > Screen.width ||
                            screenPos.y < 0 || screenPos.y > Screen.height;

            if (outScreen)
            {
                label.style.display = DisplayStyle.None;
                return;
            }

            label.style.display = DisplayStyle.Flex;

            var uiPos = RuntimePanelUtils.ScreenToPanel(panel,
                new Vector2(screenPos.x, Screen.height - screenPos.y));

            label.style.left = uiPos.x - label.resolvedStyle.width * 0.5f;
            label.style.top = uiPos.y - 100f;
        }
    }
}
