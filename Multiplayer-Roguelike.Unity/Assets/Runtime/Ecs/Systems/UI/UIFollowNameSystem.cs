using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UINameCleanerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, NameComponent, DeathTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var uiComponent = _buffer.Components1[i];

            uiComponent.Elements[UIConstants.Nickname]?.RemoveFromHierarchy();
            uiComponent.Elements.Remove(UIConstants.Nickname);
            ComponentManager.RemoveComponent<NameComponent>(entityId);
        }
    }

    public class UIFollowNameSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<UIComponent, NameComponent, PositionComponent, AliveTagComponent> _buffer = new();
        private readonly Camera _camera = Camera.main;

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var uiComponent = _buffer.Components1[i];
            var nameComponent = _buffer.Components2[i];
            var positionComponent = _buffer.Components3[i];

            var hasLabel = uiComponent.Elements.ContainsKey(UIConstants.Nickname);

            if (!hasLabel)
            {
                return;
            }

            var label = uiComponent.Elements[UIConstants.Nickname];

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
    }
}
