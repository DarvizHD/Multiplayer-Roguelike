using System.Collections.Generic;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawHealthSystem : BaseSystem
    {
        private QueryBuffer<HealthComponent, PositionComponent, EnemyTagComponent> _buffer = new();
        private readonly Dictionary<int, ProgressBar> _bars = new();

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

                if (!_bars.TryGetValue(entityId, out var bar))
                {
                    bar = CreateBar(entityId);
                }

                var screenPos = _camera.WorldToScreenPoint(positionComponent.Position);

                var outScreen = screenPos.z <= 0 ||
                                screenPos.x < 0 || screenPos.x > Screen.width ||
                                screenPos.y < 0 || screenPos.y > Screen.height;

                if (outScreen)
                {
                    bar.style.display = DisplayStyle.None;
                    continue;
                }

                bar.style.display = DisplayStyle.Flex;

                var x = screenPos.x - bar.resolvedStyle.width * 0.5f;
                var y = Screen.height - screenPos.y - 70f;

                bar.style.left = x;
                bar.style.top = y;

                bar.value = healthComponent.CurrentHealth;
                bar.highValue = healthComponent.MaxHealth;
            }
        }

        public void Destroy()
        {
            foreach (var bar in _bars.Values)
            {
                _uiHudView.WorldHudRoot.Remove(bar);
            }
        }

        private ProgressBar CreateBar(int entityId)
        {
            var root = _uiHudView.HealthAsset.CloneTree();
            var bar = root.Q<ProgressBar>("health-bar");

            bar.style.position = Position.Absolute;
            bar.highValue = 100f;

            _uiHudView.WorldHudRoot.Add(bar);
            _bars[entityId] = bar;

            return bar;
        }
    }
}
