using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using Shared.Models.GameSession;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UIDrawWaveSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<LocalControllableTag> _buffer = new();
        private readonly Label _waveLabel;
        private readonly GameSessionSharedModel _model;

        public UIDrawWaveSystem(UIHudView uiHudView, GameSessionSharedModel model)
        {
            var infoPanel = uiHudView.HudRoot.Q<VisualElement>("info-panel");
            var wavePanel = infoPanel.Q<VisualElement>("current-wave");

            _waveLabel = wavePanel.Q<Label>("wave-value");
            _model = model;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            _waveLabel.text =  _model.WaveNumber.Value.ToString();
        }
    }
}
