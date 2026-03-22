using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using Shared.Models.GameSession;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawTimerSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<LocalControllableTag> _buffer = new();

        private readonly Label _timerLabel;
        private readonly GameSessionSharedModel _model;


        public UIDrawTimerSystem(UIHudView uiHudView, GameSessionSharedModel model)
        {
            var infoPanel = uiHudView.HudRoot.Q<VisualElement>("info-panel");
            var startTime = infoPanel.Q<VisualElement>("evacuation-timer");

            _timerLabel = startTime.Q<Label>("time-value");
            _model = model;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            _timerLabel.text = _model.SessionTime.Value;
        }
    }
}
