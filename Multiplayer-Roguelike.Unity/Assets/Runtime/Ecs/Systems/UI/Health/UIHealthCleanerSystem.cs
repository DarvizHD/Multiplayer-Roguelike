using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;

namespace Runtime.ECS.Systems.UI.Health
{
    public class UIHealthCleanerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, DeathTagComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var uiComponent = _buffer.Components1[i];

            var existsHealthBar = uiComponent.Elements.ContainsKey(UIConstants.HealthBar);

            if (existsHealthBar)
            {
                uiComponent.Elements[UIConstants.HealthBar]?.RemoveFromHierarchy();
                uiComponent.Elements.Remove(UIConstants.HealthBar);
            }
        }
    }
}
