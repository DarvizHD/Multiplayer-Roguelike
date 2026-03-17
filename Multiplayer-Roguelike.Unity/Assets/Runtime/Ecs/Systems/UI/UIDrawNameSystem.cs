using System.Collections.Generic;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawNameSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<NameComponent, PositionComponent> _buffer = new();
        private readonly Dictionary<int, Label> _labels = new();
        private readonly UIHudView _uiHudView;
        private readonly Camera _camera;

        public UIDrawNameSystem(UIHudView uiHudView)
        {
            _uiHudView = uiHudView;
            _camera = Camera.main;
        }


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var nameComponent = _buffer.Components1[i];
            var positionComponent = _buffer.Components2[i];

            if (!_labels.TryGetValue(entityId, out var label))
            {
                label = CreateLabel(entityId, nameComponent.Name);
            }

            var screenPosition = _camera.WorldToScreenPoint(positionComponent.Position);

            var outScreen = screenPosition.z <= 0 ||
                            screenPosition.x < 0 || screenPosition.x > Screen.width ||
                            screenPosition.y < 0 || screenPosition.y > Screen.height;

            if (outScreen)
            {
                label.style.display = DisplayStyle.None;
                return;
            }

            label.style.display = DisplayStyle.Flex;

            var x = screenPosition.x - label.resolvedStyle.width * 0.5f;
            var y = Screen.height - screenPosition.y - 100f;

            label.style.left = x;
            label.style.top = y;
        }

        public void Destroy()
        {
            foreach (var label in _labels.Values)
            {
                _uiHudView.WorldHudRoot.Remove(label);
            }
        }

        private Label CreateLabel(int entityId, string name)
        {
            var root = _uiHudView.NameAsset.CloneTree();
            var label = root.Q<Label>("nickname");

            label.text = name;
            label.style.position = Position.Absolute;

            _uiHudView.WorldHudRoot.Add(label);
            _labels[entityId] = label;

            return label;
        }
    }
}
