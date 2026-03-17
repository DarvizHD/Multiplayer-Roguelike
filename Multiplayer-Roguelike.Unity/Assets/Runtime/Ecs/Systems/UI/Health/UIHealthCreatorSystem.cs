using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Ecs.Systems.UI;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.ECS.Systems.UI.Health
{
    public class UIHealthCreatorSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<UIComponent, HealthComponent, AliveTagComponent, EnemyTagComponent> _buffer = new();

        private readonly UIHudView _uiHudView;

        public UIHealthCreatorSystem(UIHudView uiHudView)
        {
            _uiHudView = uiHudView;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var uiComponent = _buffer.Components1[i];
            var healthComponent = _buffer.Components2[i];

            var healthBarExists = uiComponent.Elements.Contains(UIConstants.HealthBar);

            if (healthBarExists)
            {
                return;
            }

            var id =  $"{UIConstants.HealthBar}_{entityId}";

            uiComponent.Elements.Add(UIConstants.HealthBar);

            CreateBar(id);
        }

        private ProgressBar CreateBar(string name)
        {
            var root = _uiHudView.HealthAsset.CloneTree();
            var bar = root.Q<ProgressBar>("health-bar");

            bar.name = name;
            bar.style.position = Position.Absolute;
            bar.highValue = 100f;

            _uiHudView.WorldHudRoot.Add(bar);

            return bar;
        }
    }
}
