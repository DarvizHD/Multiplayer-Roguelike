using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Tags;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawCurrentPlayerHealth : BaseSystem
    {
        private QueryBuffer<HealthComponent, LocalControllableTag, PlayerTagComponent> _buffer = new();
        private readonly ProgressBar _health;

        public UIDrawCurrentPlayerHealth(UIHudView hudView)
        {
            _health = hudView.HudRoot.Q<ProgressBar>("health-bar");
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var health = _buffer.Components1[i];
                _health.value = health.CurrentHealth;
                _health.highValue = health.MaxHealth;
            }
        }
    }
}
