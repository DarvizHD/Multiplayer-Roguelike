using System.Collections.Generic;
using Runtime.ECS.Components.Health;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Components.Tags;
using Runtime.ECS.Components.UI;
using Runtime.ECS.Core;
using Runtime.ECS.Systems;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawTeammates : BaseSystem
    {
        private QueryBuffer<PlayerTagComponent, NameComponent, HealthComponent, NetworkControllableTag> _buffer = new();

        private readonly VisualTreeAsset _teammatePanelAsset;
        private readonly VisualElement _container;
        private readonly Dictionary<ushort, ProgressBar> _panels = new();

        public UIDrawTeammates(UIHudView hudView)
        {
            var panel = hudView.HudRoot.Q<VisualElement>("teammates-panel");
            _container = panel.Q<VisualElement>("container");
            _teammatePanelAsset = hudView.TeammateAsset;
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
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
