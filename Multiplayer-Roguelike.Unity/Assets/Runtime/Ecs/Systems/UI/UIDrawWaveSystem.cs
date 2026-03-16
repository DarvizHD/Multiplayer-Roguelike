using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using Shared.Models.GameSession;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawWaveSystem : BaseSystem
    {
        private readonly Label _waveLabel;
        private readonly GameSessionSharedModel _model;

        public UIDrawWaveSystem(UIHudView uiHudView, GameSessionSharedModel model)
        {
            var infoPanel = uiHudView.HudRoot.Q<VisualElement>("info-panel");
            var wavePanel = infoPanel.Q<VisualElement>("current-wave");

            _waveLabel = wavePanel.Q<Label>("wave-value");
            _model = model;
        }

        public override void Update(float deltaTime)
        {
            _waveLabel.text =  _model.WaveNumber.Value.ToString();
        }
    }
}
