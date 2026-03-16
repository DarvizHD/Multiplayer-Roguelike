using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using Shared.Models.GameSession;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawTimerSystem : BaseSystem
    {
        private readonly Label _timerLabel;
        private readonly GameSessionSharedModel _model;

        public UIDrawTimerSystem(UIHudView uiHudView, GameSessionSharedModel model)
        {
            var infoPanel = uiHudView.HudRoot.Q<VisualElement>("info-panel");
            var startTime = infoPanel.Q<VisualElement>("evacuation-timer");

            _timerLabel = startTime.Q<Label>("time-value");
            _model = model;
        }

        public override void Update(float deltaTime)
        {
            _timerLabel.text = _model.SessionTime.Value;
        }
    }
}
