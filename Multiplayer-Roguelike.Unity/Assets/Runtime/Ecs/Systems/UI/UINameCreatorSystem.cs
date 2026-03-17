using Runtime.Constants;
using Runtime.Ecs.Components.Health;
using Runtime.Ecs.Components.UI;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.UI.HUD;
using UnityEngine.UIElements;

namespace Runtime.Ecs.Systems.UI
{
    public class UINameCreatorSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<UIComponent, NameComponent, AliveTagComponent> _buffer = new();

        private readonly UIHudView _uiHudView;

        public UINameCreatorSystem(UIHudView uiHudView)
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
            var nameComponent = _buffer.Components2[i];
            var aliveTagComponent = _buffer.Components3[i];

            var nameExists = uiComponent.Elements.ContainsKey(UIConstants.Nickname);

            if (nameExists)
            {
                return;
            }

            uiComponent.Elements[UIConstants.Nickname] = CreateLabel(nameComponent.Name);
        }


        private Label CreateLabel(string name)
        {
            var root = _uiHudView.NameAsset.CloneTree();
            var label = root.Q<Label>("nickname");

            label.text = name;
            label.style.position = Position.Absolute;

            _uiHudView.WorldHudRoot.Add(label);
            return label;
        }
    }
}
