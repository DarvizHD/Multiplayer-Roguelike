using System.Collections.Generic;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawTeammatesSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;
        private QueryBuffer<PlayerTagComponent, NameComponent, HealthComponent, NetworkControllableTag> _buffer = new();

        private readonly VisualTreeAsset _teammatePanelAsset;
        private readonly VisualElement _container;
        private readonly VisualElement _root;
        private readonly Dictionary<ushort, ProgressBar> _panels = new();

        public UIDrawTeammatesSystem(UIHudView hudView)
        {
            _root = hudView.HudRoot.Q<VisualElement>("teammates-panel");
            _container = _root.Q<VisualElement>("container");
            _teammatePanelAsset = hudView.TeammateAsset;
        }


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);

            _root.style.display = _buffer.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var nameComponent = _buffer.Components2[i];
            var healthComponent = _buffer.Components3[i];

            if (!_panels.TryGetValue(entityId, out var bar))
            {
                bar = CreatePanel(entityId, nameComponent.Name);
            }

            bar.value = healthComponent.CurrentHealth;
            bar.highValue = healthComponent.MaxHealth;
        }

        public void Destroy()
        {
            foreach (var panel in _panels.Values)
            {
                _container.Remove(panel);
            }
        }

        private ProgressBar CreatePanel(ushort entityId, string name)
        {
            var bar = _teammatePanelAsset.CloneTree().Q<ProgressBar>("teammate-health-bar");
            bar.title = name;
            _container.Add(bar);
            _panels[entityId] = bar;
            return bar;
        }
    }
}
