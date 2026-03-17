using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Health
{
    public class UIHealthCleanerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, DeathTagComponent> _buffer = new();

        private readonly UIHudView _hudView;

        public UIHealthCleanerSystem(UIHudView hudView)
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

            var existsHealthBar = uiComponent.Elements.Contains(UIConstants.HealthBar);

            if (existsHealthBar)
            {
                var id = $"{UIConstants.HealthBar}_{entityId}";

                _hudView.WorldHudRoot.Q<ProgressBar>(id).RemoveFromHierarchy();

                uiComponent.Elements.Remove(UIConstants.HealthBar);
            }
        }
    }
}
