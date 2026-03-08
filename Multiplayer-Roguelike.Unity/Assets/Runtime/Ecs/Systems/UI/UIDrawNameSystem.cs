using System.Collections.Generic;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Core;
using Runtime.ECS.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.HUD
{
    public class UIDrawNameSystem : BaseSystem
    {
        private QueryBuffer<NameComponent, PositionComponent> _buffer = new();

        private readonly Dictionary<int, TextElement> _labels = new();

        private readonly UIHudView _uiHudView;
        private readonly Camera _camera;

        public UIDrawNameSystem(UIHudView uiHudView)
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
                var nameComponent = _buffer.Components1[i];
                var positionComponent = _buffer.Components2[i];

                if (!_labels.TryGetValue(entityId, out var label))
                {
                    label = new Label()
                    {
                        name = $"entity-{entityId}",
                        style =
                        {
                            position = Position.Absolute,
                            unityTextAlign =  TextAnchor.MiddleCenter,
                        },
                        text = nameComponent.Name
                    };

                    label.AddToClassList("nickname");
                    _labels[entityId] = label;
                    _uiHudView.Root.Add(label);
                }

                var worldPosition = positionComponent.Position;
                var screenPosition = _camera.WorldToScreenPoint(worldPosition);

                bool outScreen = screenPosition.z <= 0 ||
                                 screenPosition.x < 0 || screenPosition.x > Screen.width ||
                                 screenPosition.y < 0 || screenPosition.y > Screen.height;

                if (outScreen)
                {
                    label.style.display = DisplayStyle.None;
                    continue;
                }

                label.style.display = DisplayStyle.Flex;

                float offsetY = 30f;

                var x = screenPosition.x - label.resolvedStyle.width * 0.5f;
                var y = Screen.height - screenPosition.y + offsetY;

                label.style.left = x;
                label.style.top = y;
            }
        }
    }
}
