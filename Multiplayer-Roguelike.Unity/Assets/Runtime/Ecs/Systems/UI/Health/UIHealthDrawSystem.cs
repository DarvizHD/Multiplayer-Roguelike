using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Health
{
    public class UIHealthDrawSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<UIComponent, HealthComponent, PositionComponent, EnemyTagComponent, AliveTagComponent> _buffer = new();

        private readonly Camera _camera = Camera.main;

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

            var hasHealthBar = uiComponent.Elements.ContainsKey(UIConstants.HealthBar);

            if (!hasHealthBar)
            {
                return;
            }

            var healthBar = (ProgressBar) uiComponent.Elements[UIConstants.HealthBar];

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

            var x = screenPos.x - healthBar.resolvedStyle.width * 0.5f;
            var y = Screen.height - screenPos.y - 70f;

            healthBar.style.left = x;
            healthBar.style.top = y;

            healthBar.value = healthComponent.CurrentHealth;
            healthBar.highValue = healthComponent.MaxHealth;
        }
    }
}
