using System.Collections.Generic;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawHealthSystem : BaseSystem
    {
        private QueryBuffer<HealthComponent, PositionComponent> _buffer = new();
        private readonly Dictionary<int, VisualElement> _bars = new();
        private readonly Dictionary<int, VisualElement> _fills = new();

        private readonly UIHudView _uiHudView;
        private readonly Camera _camera;

        public UIDrawHealthSystem(UIHudView uiHudView)
        {
            _uiHudView = uiHudView;
            _camera = Camera.main;
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var healthComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];

                if (!_bars.TryGetValue(entityId, out var bar) || !_fills.TryGetValue(entityId, out var fill))
                {
                    bar = new VisualElement
                    {
                        name = $"health-{entityId}",
                        style =
                        {
                            position = Position.Absolute
                        }
                    };

                    fill = new VisualElement
                    {
                        name = "fill"
                    };

                    bar.AddToClassList("health-bar");
                    bar.Add(fill);

                    _bars[entityId] = bar;
                    _fills[entityId] = fill;
                    _uiHudView.Root.Add(bar);
                }

                var screenPos = _camera.WorldToScreenPoint(positionComponent.Position);

                var outScreen = screenPos.z <= 0 ||
                                screenPos.x < 0 || screenPos.x > Screen.width ||
                                screenPos.y < 0 || screenPos.y > Screen.height;

                bar.style.display = outScreen ? DisplayStyle.None : DisplayStyle.Flex;


                if (outScreen)
                {
                    bar.style.display = DisplayStyle.None;
                    continue;
                }

                var offsetY = 20f;

                var x = screenPos.x - bar.resolvedStyle.width * 0.5f;
                var y = Screen.height - screenPos.y + offsetY;

                bar.style.left = x;
                bar.style.top = y;

                var healthPercent = Mathf.Clamp01(healthComponent.CurrentHealth / healthComponent.MaxHealth);
                fill.style.width = new Length(healthPercent * 100, LengthUnit.Percent);
            }
        }
    }
}
