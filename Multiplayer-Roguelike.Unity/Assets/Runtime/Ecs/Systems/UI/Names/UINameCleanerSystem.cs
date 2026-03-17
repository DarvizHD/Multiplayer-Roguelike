using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Names
{
    public class UINameCleanerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, NameComponent, DeathTagComponent> _buffer = new();

        private readonly UIHudView _hudView;

        public UINameCleanerSystem(UIHudView hudView)
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
            var nameComponent = _buffer.Components2[i];

            var id = $"{UIConstants.Nickname}_{entityId}";

            var existsNickname = uiComponent.Elements.Contains(UIConstants.Nickname);

            if (existsNickname)
            {
                _hudView.WorldHudRoot.Q(id)?.RemoveFromHierarchy();
                uiComponent.Elements.Remove(UIConstants.Nickname);
            }
        }
    }
}
