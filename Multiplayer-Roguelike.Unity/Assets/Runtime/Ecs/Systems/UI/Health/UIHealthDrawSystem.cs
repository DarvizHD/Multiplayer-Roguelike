using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Health
{
    public class UIHealthDrawSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<UIComponent, HealthComponent, PositionComponent, EnemyTagComponent, AliveTagComponent> _buffer = new();

        private readonly Camera _camera = Camera.main;
        private readonly UIHudView _hudView;

        public UIHealthDrawSystem(UIHudView hudView)
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
            var healthComponent = _buffer.Components2[i];
            var positionComponent = _buffer.Components3[i];

            if (!uiComponent.Elements.Contains(UIConstants.HealthBar))
            {
                return;
            }

            var id = $"{UIConstants.HealthBar}_{entityId}";
            var healthBar = _hudView.WorldHudRoot.Q<ProgressBar>(id);

            if (healthBar == null)
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
                healthBar.style.display = DisplayStyle.None;
                return;
            }

            healthBar.style.display = DisplayStyle.Flex;

            var uiPos = RuntimePanelUtils.ScreenToPanel(panel,
                new Vector2(screenPos.x, Screen.height - screenPos.y));

            healthBar.style.left = uiPos.x - healthBar.resolvedStyle.width * 0.5f;
            healthBar.style.top = uiPos.y - 70f;

            healthBar.value = healthComponent.CurrentHealth;
            healthBar.highValue = healthComponent.MaxHealth;
        }
    }
}
