using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;

namespace Runtime.ECS.Systems.UI.Names
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
}
